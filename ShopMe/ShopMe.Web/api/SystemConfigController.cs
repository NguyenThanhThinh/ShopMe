using AutoMapper;
using ShopMe.Business;
using ShopMe.Domains;
using ShopMe.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/systemconfig")]
    [Authorize]
    public class SystemConfigController : ApiControllerBase
    {
        private readonly ICommonBusiness _commonService;
        public SystemConfigController(IErrorBusiness errorService, ICommonBusiness commonService) : base(errorService)
        {
            _commonService = commonService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getshopinfo")]
        public HttpResponseMessage GetShopInfo(HttpRequestMessage request)
        {
            var shopInfo = new ShopInfoViewModel();
            shopInfo.ShopName = _commonService.GetSystemConfig("ShopName").ValueString;
            shopInfo.ShopAddress = _commonService.GetSystemConfig("ShopAddress").ValueString;
            shopInfo.ShopPhone = _commonService.GetSystemConfig("ShopPhone").ValueString;

            var response = request.CreateResponse(HttpStatusCode.OK, shopInfo);
            return response;
        }

        [Route("getorderstatus")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetOrderStatusConfig(HttpRequestMessage request, string listCode)
        {
            var list = new JavaScriptSerializer().Deserialize<List<string>>(listCode);
            var model = _commonService.GetAllSystemConfig().Where(x => list.Contains(x.Code));
            var responseData = Mapper.Map<IEnumerable<SystemConfig>, IEnumerable<SystemConfigViewModel>>(model);
            var response = request.CreateResponse(HttpStatusCode.OK, responseData);
            return response;
        }
    }
}
