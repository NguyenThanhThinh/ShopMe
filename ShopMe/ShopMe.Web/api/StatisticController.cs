using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShopMe.Business;
using ShopMe.Domains;

namespace ShopMe.Web.api
{
    [Authorize(Roles = "Access")]
    [RoutePrefix("api/statistic")]
    public class StatisticController : ApiControllerBase
    {
        private readonly IStatisticBusiness _statisticBusiness;
        public StatisticController(IErrorBusiness errorService, IStatisticBusiness StatisticBusiness) : base(errorService)
        {
            this._statisticBusiness = StatisticBusiness;
        }

        [Route("getrevenue")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatistic(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticBusiness.GetStatisticPriceQuantity(fromDate, toDate);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getproductstatistic")]
        [HttpGet]
        public HttpResponseMessage GetProductStatistic(HttpRequestMessage request, string fromDate, string toDate, int sort, int top)
        {
            return CreateHttpResponse(request, () =>
            {
                //top = -1 is getall
                var model = new List<ProductStatisticViewModel>();
                if (sort == (int)DefaultListSort.Poorly_Selling_Product)           //Poorly selling product  
                {
                    model.AddRange((top == -1) ? _statisticBusiness.GetProductStatistic(fromDate, toDate).OrderBy(x => x.Quantities) : _statisticBusiness.GetProductStatistic(fromDate, toDate).OrderBy(x => x.Quantities).Take(top));
                }
                else if (sort == (int)DefaultListSort.More_Selling_Product)  //Selling products
                {
                    model.AddRange((top == -1) ? _statisticBusiness.GetProductStatistic(fromDate, toDate).OrderByDescending(x => x.Quantities) : _statisticBusiness.GetProductStatistic(fromDate, toDate).OrderByDescending(x => x.Quantities).Take(top));
                }
                else if (sort == (int)DefaultListSort.Benefit_Low)     //Benefit low
                {
                    model.AddRange((top == -1) ? _statisticBusiness.GetProductStatistic(fromDate, toDate).OrderBy(x => x.Benefit) : _statisticBusiness.GetProductStatistic(fromDate, toDate).OrderBy(x => x.Benefit).Take(top));
                }
                else if (sort == (int)DefaultListSort.Benefit_High)    //Benefit high
                {
                    model.AddRange((top == -1) ? _statisticBusiness.GetProductStatistic(fromDate, toDate).OrderByDescending(x => x.Benefit) : _statisticBusiness.GetProductStatistic(fromDate, toDate).OrderByDescending(x => x.Benefit).Take(top));
                }
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

    }
}
