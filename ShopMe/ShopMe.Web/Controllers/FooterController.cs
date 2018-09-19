using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using ShopMe.Business;
using ShopMe.Domains;
using ShopMe.Entities.Models;

namespace ShopMe.Web.Controllers
{
    public class FooterController : Controller
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IProductBrandBusiness _productBrandBusiness;

        public FooterController(IProductBusiness _productBusiness, IProductBrandBusiness _productBrandBusiness)
        {
            this._productBrandBusiness = _productBrandBusiness;
            this._productBusiness = _productBusiness;
        }

        // GET: Footer
        public ActionResult NewProduct()
        {
            var lastestProductModel = _productBusiness.GetLastest(3);
            var lastestProductViewModel =
                Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            return PartialView(lastestProductViewModel);
        }

        public ActionResult LikeProduct()
        {
            var likeProductModel = _productBusiness.GetProductLike(3, 3);
            var likeProductViewModel =
                Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(likeProductModel);
            return PartialView(likeProductViewModel);
        }

        public ActionResult ViewProduct()
        {
            var lastestProductModel = _productBusiness.GetProductViewMax(20, 3);
            var lastestProductViewModel =
                Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            return PartialView(lastestProductViewModel);
        }

        public ActionResult HostProduct()
        {
            var lastestProductModel = _productBusiness.HotProduct(3);
            return PartialView(lastestProductModel);
        }
    }
}