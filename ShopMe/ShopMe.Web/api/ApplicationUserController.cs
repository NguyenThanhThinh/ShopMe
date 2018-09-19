using AutoMapper;
using ShopMe.Business;
using ShopMe.Domains;
using ShopMe.Domains.Common;
using ShopMe.Domains.Extensions;
using ShopMe.Entities.Models;
using ShopMe.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/applicationUser")]
    [Authorize(Roles = "Access")]
    public class ApplicationUserController : ApiControllerBase
    {
        private ApplicationUserManager _userManager;
        private IGroupBusiness _appGroupService;
        private IRoleBusiness _appRoleService;
        public ApplicationUserController(
            IGroupBusiness appGroupService,
            IRoleBusiness appRoleService,
            ApplicationUserManager userManager,
            IErrorBusiness errorService)
            : base(errorService)
        {
            _appRoleService = appRoleService;
            _appGroupService = appGroupService;
            _userManager = userManager;
        }

        [Route("getlistpaging")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize = 10, string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var totalRow = 0;
                var model = _userManager.Users;
                totalRow = model.Count();
                var modelVm = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(pageSize == -1 ? model.OrderBy(x => x.IsDelete).ThenBy(x => x.FullName): model.OrderBy(x => x.IsDelete).ThenBy(x => x.FullName).Skip(page * pageSize).Take(pageSize));
                foreach (var user in modelVm)
                {
                    var listGroup = _appGroupService.GetListGroupByUserId(user.Id);
                    user.Groups = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupViewModel>>(listGroup);
                }
                var pagedSet = new PaginationSet<UserViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = pageSize == -1 ? 1 : (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVm
                };      
                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id)) return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");
            var user = _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var applicationUserViewModel = Mapper.Map<User, UserViewModel>(user.Result);
                var listGroup = _appGroupService.GetListGroupByUserId(applicationUserViewModel.Id);
                applicationUserViewModel.Groups = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupViewModel>>(listGroup);
                return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);
            }

        }

      
        [Route("detailuser/{username}")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage DetailByUserName(HttpRequestMessage request, string username)
        {
            if (string.IsNullOrEmpty(username)) return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(username) + " không có giá trị.");
            var user = _userManager.FindByNameAsync(username);
            if (user.Result == null)
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            else
                try
                {
                    var applicationUserViewModel = Mapper.Map<User, UserViewModel>(user.Result);
                    return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel.Id);
                }
                catch (Exception ex)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                }
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Add")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> Create(HttpRequestMessage request, UserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppUser = new User();
                newAppUser.UpdateUser(applicationUserViewModel);
                try
                {
                    newAppUser.Id = Guid.NewGuid().ToString();
                    applicationUserViewModel.Password = System.Web.Security.Membership.GeneratePassword(8, 2);
                    var result = await _userManager.CreateAsync(newAppUser, applicationUserViewModel.Password);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<UserGroup>();
                        foreach (var group in applicationUserViewModel.Groups)
                        {
                            listAppUserGroup.Add(new UserGroup()
                            {
                                GroupId = group.ID,
                                UserId = newAppUser.Id
                            });
                            //add role to user
                            var listRole = _appRoleService.GetListRoleByGroupId(group.ID);
                            foreach (var role in listRole)
                            {
                                await _userManager.RemoveFromRoleAsync(newAppUser.Id, role.Name);
                                await _userManager.AddToRoleAsync(newAppUser.Id, role.Name);
                            }
                        }
                        _appGroupService.AddUserToGroups(listAppUserGroup, newAppUser.Id);
                        try
                        {
                            //Sendmail
                            var content = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Assets/admin/templates/user_add_pass.html"));
                            content = content.Replace("{{FullName}}", applicationUserViewModel.FullName);
                            content = content.Replace("{{UserName}}", applicationUserViewModel.UserName);
                            content = content.Replace("{{Password}}", applicationUserViewModel.Password);
                            content = content.Replace("{{CurrentLink}}",ConfigHelper.GetByKey("CurrentLink") + "/admin/change_Password/" + applicationUserViewModel.UserName);

                            MailHelper.SendMail(applicationUserViewModel.Email, "Thông tin tài khoản từ ShopMe", content);
                        }
                        catch (Exception ex)
                        {
                            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                        }
                        _appGroupService.Save();
                        return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel);

                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                    }
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
                catch (Exception ex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Edit")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, UserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(applicationUserViewModel.Id);
                try
                {
                    appUser.UpdateUser(applicationUserViewModel);
                    var result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        var listAppUserGroup = new List<UserGroup>();
                        foreach (var group in applicationUserViewModel.Groups)
                        {
                            listAppUserGroup.Add(new UserGroup()
                            {
                                GroupId = group.ID,
                                UserId = applicationUserViewModel.Id
                            });
                            //add role to user
                            var listRole = _appRoleService.GetListRoleByGroupId(group.ID);
                            foreach (var role in listRole)
                            {
                                await _userManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                                await _userManager.AddToRoleAsync(appUser.Id, role.Name);
                            }
                        }
                        _appGroupService.AddUserToGroups(listAppUserGroup, applicationUserViewModel.Id);
                        _appGroupService.Save();
                        return request.CreateResponse(HttpStatusCode.OK, applicationUserViewModel); 
                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
                    }
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Delete")]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);   
            if(appUser.UserName == DefaultAccountApplication.admin.ToString() || appUser.UserName== DefaultAccountApplication.appshopme.ToString()) return request.CreateErrorResponse(HttpStatusCode.BadRequest, "Tài khoản này mặc định không được khóa.");
            appUser.IsDelete = true;                                 //Account status converted to deleted
            var result = await _userManager.UpdateAsync(appUser);
            if (result.Succeeded)
                try
                {
                    var listGroup = _appGroupService.GetListGroupByUserId(appUser.Id);
                    var listRole = new List<Role>();
                    foreach (var group in listGroup) listRole.AddRange(_appRoleService.GetListRoleByGroupId(@group.ID));
                    foreach (var role in listRole) await _userManager.RemoveFromRoleAsync(id, role.Name);
                    return request.CreateResponse(HttpStatusCode.OK, appUser);
                }
                catch (Exception ex)
                {
                    return request.CreateResponse(HttpStatusCode.BadRequest, ex);
                }
            else
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("changepass")]
        public async Task<HttpResponseMessage> ChangePassword(HttpRequestMessage request, ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            model.UserId = model.UserId.Replace("\"", "");
            var result = await _userManager.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = _userManager.FindByIdAsync(model.UserId);
                return request.CreateResponse(HttpStatusCode.OK, model.UserId);
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
            }
        }

         [HttpPost]
         [AllowAnonymous]
         [Route("changepassapp")]
         public async Task<HttpResponseMessage> ChangePassword(HttpRequestMessage request, string changePasswordViewModel)
        {
            if (!ModelState.IsValid) return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            var model = new JavaScriptSerializer().Deserialize<ChangePasswordViewModel>(changePasswordViewModel);
            model.UserId = model.UserId.Replace("\"", "");
            var result = await _userManager.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = _userManager.FindByIdAsync(model.UserId);
                return request.CreateResponse(HttpStatusCode.OK, model.UserId);
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Join(",", result.Errors));
            }
        }
    }
}
