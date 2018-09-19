using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ShopMe.Business;
using ShopMe.Domains;
using ShopMe.Entities.Models;

namespace Shop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryBusiness _productCategoryBusiness;
        private readonly IProductBusiness _productBusiness;
        private readonly IProductBrandBusiness _productBrandBusiness;
        private readonly ISliderBusiness _sliderBusiness;
        private readonly IFeedbackBusiness _feefbackBusiness;

        public HomeController(ICategoryBusiness productCategoryService,
            ISliderBusiness sliderBusiness,
            IProductBusiness productBusiness, IFeedbackBusiness feedbackBusiness,
            IProductBrandBusiness productBrandBusiness)
        {
            _productCategoryBusiness = productCategoryService;
            _sliderBusiness = sliderBusiness;
            _feefbackBusiness = feedbackBusiness;
            _productBusiness = productBusiness;
            _productBrandBusiness = productBrandBusiness;
        }

        [OutputCache(Duration = 60, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var homeViewModel = new HomeViewModel();
            var lastestProductModel = _productBusiness.GetLastest(8);
            if (Request.IsAuthenticated)
            {
                var userName = User.Identity.GetUserId();
                _feefbackBusiness.UserRating(userName);
                var listSugget = _productBusiness.Getbylist(userName);
                var product = new List<Product>();
                foreach (var item in listSugget)
                {
                    var model = _productBusiness.GetById(item.ProductID);

                    if (model != null) product.Add(model);
                }

                ViewBag.listSugget = product;
            }

            var categoryProduct_1 =
                Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productBusiness.GetListByCategory(21));
            var categoryProductBrand =
                Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productBusiness.GetListBrandID(6));
            var lastestProductViewModel =
                Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            homeViewModel.LastestProducts = lastestProductViewModel;
            homeViewModel.ProductCategoryDM = categoryProduct_1;
            homeViewModel.ProductBrandDM = categoryProductBrand;
            ViewBag.countFeedback = _feefbackBusiness.GetNumberFeedback();
            ViewBag.getShowHot = _productBusiness.getShowHot();

            return View(homeViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var model = _productCategoryBusiness.GetAll();

            var listProductCategoryViewModel =
                Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

            return PartialView(listProductCategoryViewModel);
        }

        public ActionResult _Slider()
        {
            var model = _sliderBusiness.GetAll();
            var listSliderViewModel = Mapper.Map<IEnumerable<Slide>, IEnumerable<SliderViewModel>>(model);
            return PartialView(listSliderViewModel);
        }


        [OutputCache(Duration = 3600)]
        public ActionResult Footer()
        {
            var model =
                Mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ProductBrandViewModel>>(
                    _productBrandBusiness.GetAll());
            return PartialView(model);
        }
    }
}