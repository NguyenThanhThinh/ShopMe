using AutoMapper;
using ShopMe.Business;
using ShopMe.Domains;
using ShopMe.Domains.Common;
using ShopMe.Domains.Extensions;
using ShopMe.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/product")]
    [Authorize]
    public class ProductController : ApiControllerBase
    {
        private readonly IProductBusiness _productService;
        private readonly IProductDetailBusiness _productDetailBusiness;
       

        public ProductController(IErrorBusiness errorService, IProductBusiness productService, IProductDetailBusiness productDetailBusiness) : base(errorService)
        {
            _productService = productService;
            _productDetailBusiness = productDetailBusiness;
      
        }
        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20,
             int categoryID = 0, int brandID = 0, int havePublished = 0, int showHomePage = 0, int sortQuantity = 0)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productService.GetAll(keyword);
                if (categoryID != 0 && brandID == 0)
                {
                    model = model.Where(x => x.CategoryID == categoryID);
                }
                else if (categoryID == 0 && brandID != 0)
                {
                    model = model.Where(x => x.ProductBrandID == brandID);
                }
                else if (categoryID != 0 && brandID != 0)
                {
                    model = model.Where(x => x.CategoryID == categoryID && x.ProductBrandID == brandID);
                }
            

                totalRow = model.Count();
                var query = model;
                if (sortQuantity == 1)
                {
                    query = (pageSize == -1) ? model.OrderBy(x => x.Quantity) : model.OrderBy(x => x.Quantity).Skip(page * pageSize).Take(pageSize);
                }
                else if (sortQuantity == 2)
                {
                    query = (pageSize == -1) ? model.OrderByDescending(x => x.Quantity) : model.OrderByDescending(x => x.Quantity).Skip(page * pageSize).Take(pageSize);
                }
                else
                {
                    query = (pageSize == -1) ? model.OrderBy(x => x.Name) : model.OrderBy(x => x.Name).Skip(page * pageSize).Take(pageSize);
                }
                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                var paginationSet = new PaginationSet<ProductViewModel>()
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
                var model = _productService.GetAll().OrderBy(x => x.Name);

                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }


        [Route("getproductbyid/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)

        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetById(id);

                var responseData = Mapper.Map<Product, ProductViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }
        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        [Authorize(Roles = "Add")]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVm)
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
                       var newProduct = new Product();
                        newProduct.UpdateProduct(productVm);
                        newProduct.CreatedDate = DateTime.Now;
                        newProduct.CreatedBy = User.Identity.Name;
                        _productService.Add(newProduct);
                        _productService.Save();
                        var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);
                        response = request.CreateResponse(HttpStatusCode.Created, responseData);

                  

                }
                return response;
            });
        }


        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "Edit")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productCategoryVm)
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
                    var dbProductCategory = _productService.GetById(productCategoryVm.ProductID);

                    dbProductCategory.UpdateProduct(productCategoryVm);
                    dbProductCategory.UpdatedDate = DateTime.Now;
            
                    dbProductCategory.UpdatedBy = User.Identity.Name;
                    _productService.Update(dbProductCategory);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(dbProductCategory);
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
                    var oldProductCategory = _productService.Delete(id);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("getlatestproduct")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetLatestProduct(HttpRequestMessage request, int top)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetLastest(top);
                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("gethethang")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetHetHang(HttpRequestMessage request, int top)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetHetHang(top);
                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
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
                var model = _productService.CountProduct();  
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }


    }
}
