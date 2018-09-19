using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShopMe.Business;
using ShopMe.Entities.Models;
using ShopMe.Domains;
using AutoMapper;
using ShopMe.Domains.Extensions;
using ShopMe.Domains.Common;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/slider")]
    public class SlideController : ApiControllerBase
    {
       readonly ISliderBusiness _slideBusiness;
        public SlideController(IErrorBusiness errorService,ISliderBusiness SlideBusiness) : base(errorService)
        {
            this._slideBusiness = SlideBusiness;
        }
        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _slideBusiness.GetAll(keyword);

                totalRow = model.Count();
                var query = (pageSize == -1) ? model.OrderBy(x => x.ID) : model.OrderBy(x => x.ID).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Slide>, IEnumerable<SliderViewModel>>(query);

                var paginationSet = new PaginationSet<SliderViewModel>()
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
        //[Authorize(Roles = "Access")]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)

        {
            return CreateHttpResponse(request, () =>
            {
                var model = _slideBusiness.GetById(id);

                var responseData = Mapper.Map<Slide, SliderViewModel>(model);

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
                var model = _slideBusiness.GetAll().OrderBy(x => x.ID);

                var responseData = Mapper.Map<IEnumerable<Slide>, IEnumerable<SliderViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        //[Authorize(Roles = "Add")]
        public HttpResponseMessage Create(HttpRequestMessage request, SliderViewModel productSlideVm)
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
                    var newProductSlide = new Slide();
                    newProductSlide.UpdateSlider(productSlideVm);
                    //newProductSlide.CreatedDate = DateTime.Now;
                    //newProductSlide.CreatedBy = User.Identity.Name;
                    _slideBusiness.Add(newProductSlide);
                    _slideBusiness.Save();

                    var responseData = Mapper.Map<Slide, SliderViewModel>(newProductSlide);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "Edit")]
        public HttpResponseMessage Update(HttpRequestMessage request, SliderViewModel productSlideVm)
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
                    var dbProductSlide = _slideBusiness.GetById(productSlideVm.ID);

                    dbProductSlide.UpdateSlider(productSlideVm);
                    //dbProductSlide.UpdatedDate = DateTime.Now;
                    //dbProductSlide.UpdatedBy = User.Identity.Name;
                    _slideBusiness.Update(dbProductSlide);
                    _slideBusiness.Save();

                    var responseData = Mapper.Map<Slide, SliderViewModel>(dbProductSlide);
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
                    ////Delete products in product Slide 
                    //var listProduct = _productService.GetListProductBySlideId(id);
                    //if (listProduct.Count() > 0)
                    //{
                    //    foreach (var product in listProduct)
                    //    {
                    //        _productService.Delete(product.ID);
                    //    }
                    //}
                    //Delete product Slide
                    var oldProductSlide = _slideBusiness.Delete(id);
                    _slideBusiness.Save();

                    var responseData = Mapper.Map<Slide, SliderViewModel>(oldProductSlide);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

    }
}
