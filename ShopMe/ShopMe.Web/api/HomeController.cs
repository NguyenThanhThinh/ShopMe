using System.Web.Http;
using ShopMe.Business;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/home")]
    [Authorize]
    public class HomeController : ApiControllerBase
    {
        private IErrorBusiness _errorbusiness;

        public HomeController(IErrorBusiness errorbusiness) : base(errorbusiness)
        {
            _errorbusiness = errorbusiness;
        }

        [HttpGet]
        [Route("TestMethod")]
        public string TestMethod()
        {
            return "Hello ";
        }
    }
}