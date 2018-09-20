using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PagedList;
using ShopMe.Business;
using ShopMe.Domains;
using ShopMe.Entities.Models;

namespace Shop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductBusiness _productBusiness;
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly IUserRatingBusiness _userRatingBusiness;
        private readonly IFeedbackBusiness _feedbackBusiness;

        public ProductController(IProductBusiness productBusiness, ICategoryBusiness categoryBusiness,
            IUserRatingBusiness userRatingBusiness, IFeedbackBusiness feedbackBusiness)
        {
            _productBusiness = productBusiness;
            _categoryBusiness = categoryBusiness;
            _userRatingBusiness = userRatingBusiness;
            _feedbackBusiness = feedbackBusiness;
        }

        // GET: Product
        [Route("{Alias}.sp-{ProductID}.html")]
        public ActionResult Detail(int ProductID)
        {
            if (ProductID != 0) Session["ProductID"] = ProductID;

            var getProduct = _productBusiness.GetById(ProductID);
            var viewProduct = Mapper.Map<Product, ProductViewModel>(getProduct);
            var getListProductCategory =
                Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(
                    _productBusiness.GetListByCategory(viewProduct.CategoryID));
            _productBusiness.IncreaseView(ProductID);
            _productBusiness.Save();

            var listProductLike =
                Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productBusiness.GetProductLike(1, 8));
            ViewBag.GetListRating =
                Mapper.Map<IEnumerable<UserRating>, IEnumerable<UserRatingViewModel>>(
                    _userRatingBusiness.GetListRatingByProduct(ProductID));
            ViewBag.GetListBCU = Mapper.Map<Product, ProductViewModel>(_productBusiness.GetAllCBU(ProductID));
            ViewBag.ProductLike = listProductLike;
            ViewBag.listProductCategory = getListProductCategory;

            return View(viewProduct);
        }


        [Route("{alias}.pc-{id}.html")]
        public ActionResult Category(int id, int? page, int? sh, string sort = "")
        {
            var _categories = _categoryBusiness.GetById(id);

            //get all childrend 
            var childrend = (List<ProductCategory>) _categoryBusiness.GetAllChildrend(_categories.CategoryID);

            if (childrend.Count > 0)
            {
                var pageNumber = page ?? 1;
                var pageSize = sh ?? 12;

                var temp = new List<Product>();
                foreach (var item in childrend)
                {
                    var productModel =
                        _productBusiness.GetListProductByCategoryIdPaging(item.CategoryID, page, pageSize, sort);
                    temp.AddRange(productModel);
                }


                var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(temp);


                var category = _categoryBusiness.GetById(id);
                ViewBag.sh = sh;
                ViewBag.id = id;
                ViewBag.sort = sort;
                ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);

                return View(productViewModel.ToPagedList(pageNumber, pageSize));
            }


            else
            {
                //int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
                var pageNumber = page ?? 1;
                var pageSize = sh ?? 12;
                var productModel = _productBusiness.GetListProductByCategoryIdPaging(id, page, pageSize, sort);
                var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);


                var category = _categoryBusiness.GetById(id);
                ViewBag.sh = sh;
                ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
                ViewBag.sort = sort;
                ViewBag.id = id;
                return View(productViewModel.ToPagedList(pageNumber, pageSize));
            }
        }


        [Route("tim-kiem.html")]
        public ActionResult Search(string keyword, int? page, int? sh, string sort = "")
        {
            var pageNumber = page ?? 1;
            var pageSize = sh ?? 12;
            var productModel = _productBusiness.Search(keyword, page, pageSize, sort);
            var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            ViewBag.sort = sort;
            ViewBag.sh = sh;
            ViewBag.Keyword = keyword;


            return View(productViewModel.ToPagedList(pageNumber, pageSize));
        }

        [OutputCache(Duration = 3600)]
        public ActionResult _OptionCategory()
        {
            var optionCategory = _categoryBusiness.GetAll();
            var model = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(optionCategory);
            return PartialView(model);
        }

        public JsonResult GetListProductByName(string keyword)
        {
            var model = _productBusiness.GetListProductByName(keyword);

            return Json(new
            {
                data = model
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Test()
        {
            //_feedbackBusiness.UserRating("8cdd40c0-5b29-4240-a0c3-08bd5bb771fb");

            return View();
        }

        [HttpPost]
        public JsonResult UserRangting(int product, int rating, string title, string content)
        {
            if (Request.IsAuthenticated)
            {
                var userName = User.Identity.GetUserId();
                var model = _userRatingBusiness.UserRating(product, userName, rating);
                _feedbackBusiness.UserRating(userName);
                if (model != null)
                {
                    var fee = new Feedback
                    {
                        ProductID = product,
                        UserID = userName,
                        Content = content,
                        Title = title,
                        Status = false,
                        CreatedDate = DateTime.Now
                    };
                    var savefeedback = _feedbackBusiness.Add(fee);
                    _feedbackBusiness.Save();
                    return Json(
                        new
                        {
                            status = true,
                            data = model
                        }, JsonRequestBehavior.AllowGet
                    );
                }

                return Json(
                    new
                    {
                        status = false
                    }, JsonRequestBehavior.AllowGet
                );
            }

            return Json(
                new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet
            );
        }


        public ActionResult _GetComment()
        {
            var user = new List<UserRating>();

            var productId = Session["ProductID"];

            var model = _feedbackBusiness.GetAll((int) productId);

            var viewmap = Mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackViewModel>>(model);

            foreach (var item in model)
            {
                var getrating = _userRatingBusiness.GetmutiRating(item.ProductID, item.UserID);
                user.AddRange(getrating);
            }

            ViewBag.id = productId;

            ViewBag.userrating = user;

            return PartialView(viewmap);
        }

        public ActionResult Getexist(int productrating)
        {
            if (Request.IsAuthenticated)
            {
                var userName = User.Identity.GetUserId();
                var model = _userRatingBusiness.getExist(productrating, userName);
                if (model != null)
                    return Json(new
                    {
                        status = false,
                        message = "Bạn đã Rating sản phẩm này rồi"
                    }, JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    status = true
                });
            }

            return Json(new
            {
                status = true
            });
        }

        public ActionResult _ProductLike()
        {
            return PartialView();
        }
    }
}