using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using AutoMapper;
using ShopMe.Business;
using ShopMe.Domains;
using ShopMe.Domains.Common;
using ShopMe.Domains.Extensions;
using ShopMe.Entities.Models;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/applicationRole")]
    [Authorize(Roles = "Access")]
    public class ApplicationRoleController : ApiControllerBase
    {
        private readonly IRoleBusiness _appRoleService;

        public ApplicationRoleController(IErrorBusiness errorService,
            IRoleBusiness appRoleService) : base(errorService)
        {
            _appRoleService = appRoleService;
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
                var model = _appRoleService.GetAll(page, pageSize, out totalRow, filter);
                var modelVm = Mapper.Map<IEnumerable<Role>, IEnumerable<RoleViewModel>>(model);

                var pagedSet = new PaginationSet<RoleViewModel>
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
                var model = _appRoleService.GetAll();
                var modelVm = Mapper.Map<IEnumerable<Role>, IEnumerable<RoleViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " không có giá trị.");

            var appRole = _appRoleService.GetDetail(id);
            if (appRole == null) return request.CreateErrorResponse(HttpStatusCode.NoContent, "No group");

            return request.CreateResponse(HttpStatusCode.OK, appRole);
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Add")]
        public HttpResponseMessage Create(HttpRequestMessage request, RoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppRole = new Role();
                newAppRole.UpdateApplicationRole(applicationRoleViewModel);
                try
                {
                    _appRoleService.Add(newAppRole);
                    _appRoleService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, applicationRoleViewModel);
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
        public HttpResponseMessage Update(HttpRequestMessage request, RoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var appRole = _appRoleService.GetDetail(applicationRoleViewModel.Id);
                try
                {
                    appRole.UpdateApplicationRole(applicationRoleViewModel, "update");
                    _appRoleService.Update(appRole);
                    _appRoleService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, appRole);
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
        public HttpResponseMessage Delete(HttpRequestMessage request, string id)
        {
            _appRoleService.Delete(id);
            _appRoleService.Save();
            return request.CreateResponse(HttpStatusCode.OK, id);
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
                    var listItem = new JavaScriptSerializer().Deserialize<List<string>>(checkedList);
                    foreach (var item in listItem) _appRoleService.Delete(item);

                    _appRoleService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listItem.Count);
                }

                return response;
            });
        }
    }
}