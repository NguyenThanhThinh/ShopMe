using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using ShopMe.Entities;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ShopMe.Entities.Models;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Facebook;
using ShopMe.Business;
using ShopMe.Web.api;
using System.Linq;
using ShopMe.Domains.Common;
using System.Net.Http;
using ShopMe.Domains;
using Newtonsoft.Json;
using System.Net;
using System.Web;

[assembly: OwinStartup(typeof(ShopMe.Web.App_Start.Startup))]

namespace ShopMe.Web.App_Start
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ShopMeDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<UserManager<User>>(CreateManager);


            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                AllowInsecureHttp = true,
            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/dang-nhap.html"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) =>
                            user.GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //var facebookOptions = new FacebookAuthenticationOptions()
            //{

            //    AppId = "1299467440131386",
            //    AppSecret = "797d3af5fc5dcda854326de0210522ed",


            //    Scope = { "email" },
            //    Provider = new FacebookAuthenticationProvider
            //    {
            //        OnAuthenticated = context =>
            //        {
            //            context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
            //            return Task.FromResult(true);
            //        }
            //    }
            //};
            //app.UseFacebookAuthentication(facebookOptions);


            app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {
                AppId = "1299467440131386",
                AppSecret = "797d3af5fc5dcda854326de0210522ed",
                BackchannelHttpHandler = new FacebookBackChannelHandler(),
                Scope = {"email"},
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken",
                            context.AccessToken));
                        return Task.FromResult(true);
                    }
                }
            });


            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "437674010954-j84scptf2e0hmmc5pr81ge7qqeir38gl.apps.googleusercontent.com",
                ClientSecret = "qfJGP0kB9cg3P_2h80rpkYkO"
            });
        }

        public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
        {
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                context.Validated();
            }

            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

                if (allowedOrigin == null) allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {allowedOrigin});

                UserManager<User> userManager = context.OwinContext.GetUserManager<UserManager<User>>();
                User user;
                try
                {
                    user = await userManager.FindAsync(context.UserName, context.Password);
                }
                catch
                {
                    // Could not retrieve the user due to error.
                    context.SetError("server_error");
                    context.Rejected();
                    return;
                }

                if (user != null)
                {
                    var applicationGroupService = ServiceFactory.Get<IGroupBusiness>();
                    var listGroup = applicationGroupService.GetListGroupByUserId(user.Id);
                    if (listGroup.Any(x => x.Name == CommonConstants.Admin || x.Name == CommonConstants.Employee))
                    {
                        ClaimsIdentity identity = await userManager.CreateIdentityAsync(
                            user,
                            DefaultAuthenticationTypes.ExternalBearer);
                        context.Validated(identity);
                    }
                    else
                    {
                        context.Rejected();
                        context.SetError("invalid_group", "Bạn không phải là admin");
                    }
                }
                else
                {
                    context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không đúng.'");
                    context.Rejected();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class FacebookBackChannelHandler : HttpClientHandler
        {
            protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                var result = await base.SendAsync(request, cancellationToken);
                if (!request.RequestUri.AbsolutePath.Contains("access_token"))
                    return result;

                // For the access token we need to now deal with the fact that the response is now in JSON format, not form values. Owin looks for form values.
                var content = await result.Content.ReadAsStringAsync();
                var facebookOauthResponse = JsonConvert.DeserializeObject<FacebookOauthResponse>(content);

                var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
                outgoingQueryString.Add(nameof(facebookOauthResponse.access_token), facebookOauthResponse.access_token);
                outgoingQueryString.Add(nameof(facebookOauthResponse.expires_in),
                    facebookOauthResponse.expires_in + string.Empty);
                outgoingQueryString.Add(nameof(facebookOauthResponse.token_type), facebookOauthResponse.token_type);
                var postdata = outgoingQueryString.ToString();

                var modifiedResult = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(postdata)
                };

                return modifiedResult;
            }
        }

        private static UserManager<User> CreateManager(IdentityFactoryOptions<UserManager<User>> options,
            IOwinContext context)
        {
            var userStore = new UserStore<User>(context.Get<ShopMeDbContext>());
            var owinManager = new UserManager<User>(userStore);
            return owinManager;
        }
    }
}