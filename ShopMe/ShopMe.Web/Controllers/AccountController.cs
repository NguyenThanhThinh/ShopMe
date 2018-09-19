using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ShopMe.Domains;
using ShopMe.Domains.Common;
using ShopMe.Entities.Models;
using ShopMe.Web.App_Start;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShopMe.Web.Controllers
{
    //[RoutePrefix("Account")]
    public class AccountController : Controller
    {
        // GET: Account
        private ApplicationSignInManager _signInManager;
   
        private ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
          
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public AccountController()
        {

        }

        // GET: Account
        //public ActionResult Login(string returnUrl)
        //{
        //    ViewBag.ReturnUrl = returnUrl;
        //    return View();
        //}
        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(string NewPassword)
        {
            
                if (Request.IsAuthenticated)
                {
                    var userId = User.Identity.GetUserId();
                    var user = _userManager.FindById(userId);
                    var id = user.Id;
                    if (user == null)
                    {
                        return Json(new
                        {
                            status = false,
                            message = "tài khoản không tồn tại"


                        }, JsonRequestBehavior.AllowGet);
                    }
                   
                    var result = await UserManager.RemovePasswordAsync(id);
                    if (result.Succeeded)
                    {
                        var updatePasswordResult = await UserManager.AddPasswordAsync(id, NewPassword);
            
                        if (updatePasswordResult.Succeeded)
                        {
                            TempData["chanpass"] = "cập nhật thành công";
                            return Json(new
                            {
                                status = true,
                                message = "cập nhật thành công"


                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
               
                    return Json(new
                    {
                        status = false,
                        message = "Cập nhật không thành công"



                    });
                
           
           
        
        }

        [HttpPost]
      
        public async Task<ActionResult> Login(string username, string password)
        {

            User user = await _userManager.FindAsync(username, password);
            if (user != null)
            {
                IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationProperties props = new AuthenticationProperties();
                //props.IsPersistent = model.RememberMe;
                authenticationManager.SignIn(props, identity);
                return Json(new
                {
                    status = true,
                    message = "đăng nhập thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "tài khoản hoặc mật khẩu không đúng"
                }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return Redirect("/trang-chu.html");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, FullName = loginInfo.DefaultUserName });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return Redirect("trang-chu.html");
                }
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
       
        public async Task<ActionResult> ResetPassword(string resetEmail)
        {
          
            var user= await _userManager.FindByEmailAsync(resetEmail);


            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return Json(new
                    {
                        status = false,
                        message = "Email không tồn tại"
                    });
                }




            //string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/resetpassword.html"));
            //content = content.Replace("{{UserName}}", user.FullName);
            //content = content.Replace("{{PassWord}}", user.PasswordHash);
            //content = content.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "/dang-nhap.html");

            //MailHelper.SendMail(user.Email, "Khôi phục mật khẩu thành công", content);
          
            return Json(new
                {
                    status = true,
                    message = "Vui lòng kiểm tra email, chúng tôi đã gởi mail cho bạn"
                });

            
              
            

        }

        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            TempData["chanpass"] = null;
            return Redirect("/trang-chu.html");
        }


        [HttpPost]
        public async Task<ActionResult> Register(string FullName, string Password, string UserName, string Email)
        {

            var userByEmail = await _userManager.FindByEmailAsync(Email);
            if (userByEmail != null)
            {
                return Json(new
                {
                    status = false,
                    message = "Email đã tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }
            var userByUserName = await _userManager.FindByNameAsync(UserName);
            if (userByUserName != null)
            {
                return Json(new
                {
                    status = false,
                    message = "Tài khoản đã tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }
            var user = new User()
            {
                UserName = UserName,
                Email = Email,
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = FullName,
                //PhoneNumber =PhoneNumber,
                //Address =Address

            };

            await _userManager.CreateAsync(user, Password);


            var adminUser = await _userManager.FindByEmailAsync(Email);
            if (adminUser != null)
            {
                await _userManager.AddToRolesAsync(adminUser.Id, new string[] { "User" });

                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/client/template/newuser.html"));
                content = content.Replace("{{UserName}}", adminUser.FullName);
                content = content.Replace("{{Link}}", ConfigHelper.GetByKey("CurrentLink") + "/trang-chu.html");

                MailHelper.SendMail(adminUser.Email, "Đăng ký thành công", content);


                return Json(new
                {

                    status = true,
                    message = "Đăng ký thành công !"
                },JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new
                {
                    status = true,
                    message = "Đăng ký không  thành công !"
                }, JsonRequestBehavior.AllowGet);
            }


        }


        public static string ConvertToUnsign3(string str)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = str.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty)
                        .Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                context.RequestContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}