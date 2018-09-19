using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShopMe.Business;
using AutoMapper;
using ShopMe.Entities.Models;
using ShopMe.Domains;
using ShopMe.Domains.Common;
using ShopMe.Domains.Extensions;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/productbrand")]
    public class ProductBrandController : ApiControllerBase
    {
        private IProductBrandBusiness _productBrandBusiness;
        public ProductBrandController(IErrorBusiness errorService, IProductBrandBusiness productBrandBusiness) : base(errorService)
        {
            this._productBrandBusiness = productBrandBusiness;
        }


        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productBrandBusiness.GetAll(keyword);

                totalRow = model.Count();
                var query = (pageSize == -1) ? model.OrderBy(x => x.Name) : model.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ProductBrandViewModel>>(query);

                var paginationSet = new PaginationSet<ProductBrandViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (pageSize == -1) ? 1 : (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }
        [Route("getallparents")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productBrandBusiness.GetAll().OrderBy(x => x.Name);

                var responseData = Mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ProductBrandViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }


        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)

        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productBrandBusiness.GetById(id);

                var responseData = Mapper.Map<ProductBrand, ProductBrandViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }


        [Route("create")]
        [HttpPost]
        [Authorize(Roles = "Add")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductBrandViewModel productBrandVm)
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
                    var newProductCategory = new ProductBrand();
                    newProductCategory.UpdateBrand(productBrandVm);
                    newProductCategory.CreatedDate = DateTime.Now;
                    newProductCategory.CreatedBy = User.Identity.Name;
                    _productBrandBusiness.Add(newProductCategory);
                    _productBrandBusiness.Save();

                    var responseData = Mapper.Map<ProductBrand, ProductBrandViewModel>(newProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }



        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "Edit")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductBrandViewModel productCategoryVm)
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
                    var dbProductCategory = _productBrandBusiness.GetById(productCategoryVm.ProductBrandID);

                    dbProductCategory.UpdateBrand(productCategoryVm);
                    dbProductCategory.UpdatedDate = DateTime.Now;
                    dbProductCategory.UpdatedBy = User.Identity.Name;
                    _productBrandBusiness.Update(dbProductCategory);
                    _productBrandBusiness.Save();

                    var responseData = Mapper.Map<ProductBrand, ProductBrandViewModel>(dbProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }


        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = "Delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
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
                    //Delete productbrand
                    var oldProductCategory = _productBrandBusiness.Delete(id);
                    _productBrandBusiness.Save();

                    var responseData = Mapper.Map<ProductBrand, ProductBrandViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("count")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage CountProduct(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productBrandBusiness.CountBrand();
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
    }
}
