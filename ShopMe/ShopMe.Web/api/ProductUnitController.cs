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
    [RoutePrefix("api/productunit")]
    public class ProductUnitController : ApiControllerBase
    {
        private IProductUnitBusiness _productUnitBusiness;
        public ProductUnitController(IErrorBusiness errorService, IProductUnitBusiness productUnitBusiness) : base(errorService)
        {
            this._productUnitBusiness = productUnitBusiness;
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productUnitBusiness.GetAll(keyword);

                totalRow = model.Count();
                var query = (pageSize == -1) ? model.OrderBy(x => x.Name) : model.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<ProductUnit>, IEnumerable<ProductUnitViewModel>>(query);

                var paginationSet = new PaginationSet<ProductUnitViewModel>()
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

        [Route("getbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)

        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productUnitBusiness.GetById(id);

                var responseData = Mapper.Map<ProductUnit, ProductUnitViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

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
                var model = _productUnitBusiness.GetAll().OrderBy(x => x.Name);

                var responseData = Mapper.Map<IEnumerable<ProductUnit>, IEnumerable<ProductUnitViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        [Authorize(Roles = "Add")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductUnitViewModel productCategoryVm)
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
                    var newProductCategory = new ProductUnit();
                    newProductCategory.UpdateUnit(productCategoryVm);
                    newProductCategory.CreatedDate = DateTime.Now;
                    newProductCategory.CreatedBy = User.Identity.Name;
                    _productUnitBusiness.Add(newProductCategory);
                    _productUnitBusiness.Save();

                    var responseData = Mapper.Map<ProductUnit, ProductUnitViewModel>(newProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "Edit")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductUnitViewModel productCategoryVm)
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
                    var dbProductCategory = _productUnitBusiness.GetById(productCategoryVm.ProductUnitID);

                    dbProductCategory.UpdateUnit(productCategoryVm);
                    dbProductCategory.UpdatedDate = DateTime.Now;
                    dbProductCategory.UpdatedBy = User.Identity.Name;
                    _productUnitBusiness.Update(dbProductCategory);
                    _productUnitBusiness.Save();

                    var responseData = Mapper.Map<ProductUnit, ProductUnitViewModel>(dbProductCategory);
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
                    ////Delete products in product category 
                    //var listProduct = _productService.GetListProductByCategoryId(id);
                    //if (listProduct.Count() > 0)
                    //{
                    //    foreach (var product in listProduct)
                    //    {
                    //        _productService.Delete(product.ID);
                    //    }
                    //}
                    //Delete product category
                    var oldProductCategory = _productUnitBusiness.Delete(id);
                    _productUnitBusiness.Save();

                    var responseData = Mapper.Map<ProductUnit, ProductUnitViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
    }
}
