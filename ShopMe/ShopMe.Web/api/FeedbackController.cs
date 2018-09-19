using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShopMe.Business;
using AutoMapper;
using ShopMe.Domains.Common;
using ShopMe.Entities.Models;
using ShopMe.Domains;
using ShopMe.Domains.Extensions;
using System.Web.Script.Serialization;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/feedback")]
    public class FeedbackController : ApiControllerBase
    {
        private readonly IFeedbackBusiness _feedback;
        public FeedbackController(IErrorBusiness errorService, IFeedbackBusiness feedback) : base(errorService)
        {
            this._feedback = feedback;
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _feedback.GetAll(keyword);

                totalRow = model.Count();
                var query = (pageSize == -1) ? model.OrderBy(x => x.CreatedDate) : model.OrderBy(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(query);

                var paginationSet = new PaginationSet<FeedbackViewModel>()
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
                var model = _feedback.GetById(id);

                var responseData = Mapper.Map<Feedback, FeedbackViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getallparents")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _feedback.GetAll();

                var responseData = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getnumfeedbacknew")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetNumberFeedbackNew(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _feedback.GetNumberFeedback();
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

       

      
        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "Edit")]
        public HttpResponseMessage Update(HttpRequestMessage request, FeedbackViewModel feedbackVm)
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
                    var dbFeedback = _feedback.GetById(feedbackVm.FeedbackID);
                    try
                    {
                        dbFeedback.UpdateFeedback(feedbackVm);
                       
                        _feedback.Update(dbFeedback);

                        //Sendmail
                        
                    }
                    catch
                    {
                        response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                    _feedback.Save();

                    var responseData = Mapper.Map<Feedback, FeedbackViewModel>(dbFeedback);
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
                    var oldFeedback = _feedback.Delete(id);

                    _feedback.Save();

                    var responseData = Mapper.Map<Feedback, FeedbackViewModel>(oldFeedback);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [Authorize(Roles = "Delete")]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedFeedbacks)
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
                    var listFeedback = new JavaScriptSerializer().Deserialize<List<int>>(checkedFeedbacks);
                    foreach (var item in listFeedback)
                    {
                        _feedback.Delete(item);
                    }

                    _feedback.Save();


                    response = request.CreateResponse(HttpStatusCode.Created, listFeedback.Count);
                }

                return response;
            });
        }
    }
}
