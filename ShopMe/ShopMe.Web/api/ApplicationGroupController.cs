using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using AutoMapper;
using ShopMe.Business;
using ShopMe.Domains;
using ShopMe.Domains.Common;
using ShopMe.Domains.Extensions;
using ShopMe.Entities.Models;
using ShopMe.Web.App_Start;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/applicationGroup")]
    [Authorize(Roles = "Access")]
    public class ApplicationGroupController : ApiControllerBase
    {
        private readonly IGroupBusiness _appGroupService;
        private readonly IRoleBusiness _appRoleService;
        private readonly ApplicationUserManager _userManager;

        public ApplicationGroupController(IErrorBusiness errorService,
            IRoleBusiness appRoleService,
            ApplicationUserManager userManager,
            IGroupBusiness appGroupService) : base(errorService)
        {
            _appGroupService = appGroupService;
            _appRoleService = appRoleService;
            _userManager = userManager;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize,
            string filter = null)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var totalRow = 0;
                var model = _appGroupService.GetAll(page, pageSize, out totalRow, filter);
                var modelVm = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupViewModel>>(model);
                var pagedSet = new PaginationSet<GroupViewModel>
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int) Math.Ceiling((decimal) totalRow / pageSize),
                    Items = modelVm
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _appGroupService.GetAll().OrderBy(x => x.Description);
                var modelVm = Mapper.Map<IEnumerable<Group>, IEnumerable<GroupViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, int id)
        {
            if (id == 0) return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " is required.");
            var appGroup = _appGroupService.GetDetail(id);
            var appGroupViewModel = Mapper.Map<Group, GroupViewModel>(appGroup);
            if (appGroup == null) return request.CreateErrorResponse(HttpStatusCode.NoContent, "No group");
            var listRole = _appRoleService.GetListRoleByGroupId(appGroupViewModel.ID);
            appGroupViewModel.Roles = Mapper.Map<IEnumerable<Role>, IEnumerable<RoleViewModel>>(listRole);
            return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Add")]
        public HttpResponseMessage Create(HttpRequestMessage request, GroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppGroup = new Group();
                newAppGroup.Name = appGroupViewModel.Name;
                newAppGroup.Description = appGroupViewModel.Description;
                try
                {
                    var appGroup = _appGroupService.Add(newAppGroup);
                    _appGroupService.Save();

                    //save group
                    var listRoleGroup = new List<RoleGroup>();
                    foreach (var role in appGroupViewModel.Roles)
                        listRoleGroup.Add(new RoleGroup
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _appRoleService.Save();


                    return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }

            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Edit")]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, GroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var appGroup = _appGroupService.GetDetail(appGroupViewModel.ID);
                try
                {
                    appGroup.UpdateApplicationGroup(appGroupViewModel);
                    _appGroupService.Update(appGroup);
                    _appGroupService.Save();

                    //save group
                    var listRoleGroup = new List<RoleGroup>();
                    foreach (var role in appGroupViewModel.Roles)
                        listRoleGroup.Add(new RoleGroup
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.Id
                        });
                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _appRoleService.Save();

                    //add role to user
                    var listRole = _appRoleService.GetListRoleByGroupId(appGroup.ID);
                    var listUserInGroup = _appGroupService.GetListUserByGroupId(appGroup.ID);
                    foreach (var user in listUserInGroup)
                    {
                        var listRoleName = listRole.Select(x => x.Name).ToArray();
                        foreach (var roleName in listRoleName)
                        {
                            await _userManager.RemoveFromRoleAsync(user.Id, roleName);
                            await _userManager.AddToRoleAsync(user.Id, roleName);
                        }
                    }

                    return request.CreateResponse(HttpStatusCode.OK, appGroup);
                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }

            return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            var appGroup = _appGroupService.Delete(id);
            _appGroupService.Save();
            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }

        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "Delete")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listItem = new JavaScriptSerializer().Deserialize<List<int>>(checkedList);
                    foreach (var item in listItem) _appGroupService.Delete(item);

                    _appGroupService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listItem.Count);
                }

                return response;
            });
        }
    }
}