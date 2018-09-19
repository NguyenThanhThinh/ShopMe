using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ShopMe.Business;
using ShopMe.Domains;
using ShopMe.Domains.Common;
using ShopMe.Domains.Extensions;
using ShopMe.Entities.Models;
using ShopMe.Web.api;
using ShopMe.Web.App_Start;

namespace Shop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IOrderBusiness _orderBusiness;
        private readonly ICommonBusiness _commonBusiness;
        private readonly ApplicationUserManager _userManager;
        private readonly string merchantId = ConfigHelper.GetByKey("MerchantId");
        private readonly string merchantPassword = ConfigHelper.GetByKey("MerchantPassword");
        private readonly string merchantEmail = ConfigHelper.GetByKey("MerchantEmail");

        public ShoppingCartController(IProductBusiness productBusiness, ApplicationUserManager userManager,
            ICommonBusiness commonBusines, IOrderBusiness orderBusiness)
        {
            _productBusiness = productBusiness;
            _userManager = userManager;
            _commonBusiness = commonBusines;
            _orderBusiness = orderBusiness
                ;
        }

        // GET: ShoppingCart
        [Route("Order/Detail.html")]
        public ActionResult Index()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            ViewBag.getShowHot = _productBusiness.getShowHot();
            return View();
        }

        [Route("order/checkout.html")]
        public ActionResult Checkout()
        {
            if (Session[CommonConstants.SessionCart] == null) return RedirectToAction("Index");
            return View();
        }

        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            var cart = (List<ShoppingCartViewModel>) Session[CommonConstants.SessionCart];

            return Json(new
            {
                data = cart,
                status = true,
                summonney = SumMoney().Value.ToString("#,##"),
                sumquantity = SumQuantity().Value.ToString("#,##")
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(int productId)
        {
            var cart = (List<ShoppingCartViewModel>) Session[CommonConstants.SessionCart];
            var product = _productBusiness.GetById(productId);
            if (cart == null) cart = new List<ShoppingCartViewModel>();
            if (product.Quantity == 0)
                return Json(new
                {
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                }, JsonRequestBehavior.AllowGet);
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                    if (item.ProductId == productId)
                    {
                        if (product.Quantity > item.Quantity)
                            item.Quantity += 1;
                        else
                            return Json(new
                            {
                                status = false,
                                message = "Sản phẩm này hiện chỉ có " + product.Quantity + " sản phẩm "
                            }, JsonRequestBehavior.AllowGet);
                    }
            }
            else
            {
                var newItem = new ShoppingCartViewModel
                {
                    ProductId = productId,
                    Product = Mapper.Map<Product, ProductViewModel>(product),
                    Quantity = 1
                };
                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true,
                sumquantity = SumQuantity().Value.ToString("#,##")
            }, JsonRequestBehavior.AllowGet);
        }

        public decimal? SumMoney()
        {
            var res = Session[CommonConstants.SessionCart] as List<ShoppingCartViewModel>;
            if (res == null)
                return 0;
            return res.Sum(n => n.Quantity * n.Product.Price);
        }

        public decimal? SumQuantity()
        {
            var res = Session[CommonConstants.SessionCart] as List<ShoppingCartViewModel>;
            if (res == null)
                return 0;
            return res.Sum(n => n.Quantity);
        }

        [HttpPost]
        public JsonResult AddCartQuantity(int productId, int Quantity)
        {
            var cart = (List<ShoppingCartViewModel>) Session[CommonConstants.SessionCart];
            var product = _productBusiness.GetById(productId);
            if (cart == null) cart = new List<ShoppingCartViewModel>();
            var productQuantity = product.Quantity;
            var productid = product.ProductID;
            if (productQuantity < Quantity)
                return Json(new
                {
                    productid,
                    quantity = productQuantity,
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                }, JsonRequestBehavior.AllowGet);
            if (cart.Any(x => x.ProductId == productId))
            {
                foreach (var item in cart)
                    if (item.ProductId == productId)
                    {
                        if (productQuantity == 1) item.Quantity = Quantity;

                        item.Quantity += Quantity;

                        return Json(new
                        {
                            status = true,
                            message = "thêm sản phẩm thành công",
                            sumquantity = SumQuantity().Value.ToString("#,##")
                        }, JsonRequestBehavior.AllowGet);
                    }
            }
            else
            {
                var newItem = new ShoppingCartViewModel
                {
                    ProductId = productId,
                    Product = Mapper.Map<Product, ProductViewModel>(product),
                    Quantity = Quantity
                };
                cart.Add(newItem);
            }

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true,
                message = "thêm sản phẩm thành công",
                sumquantity = SumQuantity().Value.ToString("#,##")
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = (List<ShoppingCartViewModel>) Session[CommonConstants.SessionCart];
            if (cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductId == productId);
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true,
                    sumquantity = SumQuantity().Value.ToString("#,##")
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult Update(string cartData)
        //{
        //    var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

        //    var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
        //    foreach (var item in cartSession)
        //    {
        //        foreach (var jitem in cartViewModel)
        //        {
        //            if (item.ProductId == jitem.ProductId)
        //            {
        //                item.Quantity = jitem.Quantity;
        //            }
        //        }
        //    }

        //    Session[CommonConstants.SessionCart] = cartSession;
        //    return Json(new
        //    {
        //        status = true,
        //        sumquantity = SumMoney().Value.ToString("#,##")

        //    });
        //}

        public JsonResult getQuantity(int productId)
        {
            var cart = (List<ShoppingCartViewModel>) Session[CommonConstants.SessionCart];
            var product = _productBusiness.GetById(productId);
            if (cart == null) cart = new List<ShoppingCartViewModel>();
            //get quatity product
            var productQuantity = product.Quantity;
            var productid = product.ProductID;

            return Json(new
            {
                productid,
                quantity = productQuantity,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Update(int productId, int Quantity)
        {
            var cart = (List<ShoppingCartViewModel>) Session[CommonConstants.SessionCart];
            var product = _productBusiness.GetById(productId);
            if (cart == null) cart = new List<ShoppingCartViewModel>();
            var productQuantity = product.Quantity;
            var productid = product.ProductID;
            if (productQuantity < Quantity)
                return Json(new
                {
                    productid,
                    quantity = productQuantity,
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                }, JsonRequestBehavior.AllowGet);
            if (cart.Any(x => x.ProductId == productId))
                foreach (var item in cart)
                    if (item.ProductId == productId)
                        item.Quantity = Quantity;

            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true,
                sumquantity = SumQuantity().Value.ToString("#,##")
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true,
                sumquantity = SumMoney().Value.ToString("#,##")
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountries()
        {
            var model = _commonBusiness.GetAllProvinces();
            var getViewModel = Mapper.Map<IEnumerable<Provinces>, IEnumerable<ProvincesViewModel>>(model);
            return Json(getViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatesByCountryId(int countryId)
        {
            var model = _commonBusiness.GetByDistricts(countryId);
            var getViewModel = Mapper.Map<IEnumerable<Districts>, IEnumerable<DistrictsViewModel>>(model);
            return Json(getViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            var orderNew = new Order();
            orderNew.UpdateOrder(order);
            orderNew.CreatedDate = DateTime.Now;
            if (Request.IsAuthenticated)
            {
                orderNew.UserID = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<ShoppingCartViewModel>) Session[CommonConstants.SessionCart];
            var orderDetails = new List<OrderDetail>();
            var isSellProduct = true;
            foreach (var item in cart)
            {
                var detail = new OrderDetail
                {
                    ProductID = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                };
                orderDetails.Add(detail);
                var sellProduct = _productBusiness.KTQuantity(item.ProductId, item.Quantity);
                if (isSellProduct == false)
                    break;
            }

            if (isSellProduct)
            {
                var orderReturn = _orderBusiness.Create(ref orderNew, orderDetails);
                _productBusiness.Save();
                var content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/admin/templates/order_reply.html"));
                content = content.Replace("{{CustomerName}}", orderReturn.CustomerName);
                content = content.Replace("{{Email}}", orderReturn.CustomerEmail);
                content = content.Replace("{{OrderID}}", orderReturn.OrderID.ToString());
                MailHelper.SendMail(order.CustomerEmail, "Thông tin đơn hàng từ ShopMe", content);
                if (order.PaymentMethod == "CASH")
                {
                    TempData["mgs"] = "Cảm ơn bạn đã đặt hàng thành công.Chúng tôi sẽ liên hệ sớm nhất.";
                    return Json(new
                    {
                        paymenthod = "CASH",
                        status = true,
                        message = "Cảm ơn bạn đã đặt hàng thành công. Chúng tôi sẽ liên hệ sớm nhất.!"
                    }, JsonRequestBehavior.AllowGet);
                }

                var currentLink = ConfigHelper.GetByKey("CurrentLink");
                var info = new RequestInfo
                {
                    Merchant_id = merchantId,
                    Merchant_password = merchantPassword,
                    Receiver_email = merchantEmail,
                    cur_code = "vnd",
                    bank_code = order.BankCode,
                    Order_code = orderReturn.OrderID.ToString(),
                    Total_amount = orderDetails.Sum(x => x.Quantity * x.Price).ToString(),
                    fee_shipping = "0",
                    Discount_amount = "0",
                    order_description = "Thanh toán đơn hàng tại ShopMe",
                    return_url = currentLink + "/order/xac-nhan-don-hang.html",
                    cancel_url = currentLink + "/order/xac-nhan-huy-don-hang.html",
                    Buyer_fullname = order.CustomerName,
                    Buyer_email = order.CustomerEmail,
                    Buyer_mobile = order.CustomerMobile
                };
                var objNlChecout = new APICheckoutV3();
                var result = objNlChecout.GetUrlCheckout(info, order.PaymentMethod);
                if (result.Error_code == "00")
                {
                    TempData["mgs"] = "Cảm ơn bạn đã đặt hàng thành công.Chúng tôi sẽ liên hệ sớm nhất.";
                    return Json(new
                    {
                        status = true,
                        urlCheckout = result.Checkout_url,
                        message = result.Description
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    status = false,
                    message = result.Description
                }, JsonRequestBehavior.AllowGet);
            }

            TempData["mgs"] = "không đủ hàng , cảm ơn quý khách";
            return Json(new
            {
                status = false,
                message = "Không đủ hàng."
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("order/xac-nhan-don-hang.html")]
        public ActionResult ConfirmOrder()
        {
            var token = Request["token"];
            var info = new RequestCheckOrder
            {
                Merchant_id = merchantId,
                Merchant_password = merchantPassword,
                Token = token
            };
            var objNlChecout = new APICheckoutV3();
            var result = objNlChecout.GetTransactionDetail(info);
            if (result.errorCode == "00")
            {
                //update status order
                _orderBusiness.UpdateStatus(int.Parse(result.order_code));

                _orderBusiness.Save();
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Thanh toán thành công. Chúng tôi sẽ liên hệ lại sớm nhất.";
            }
            else
            {
                ViewBag.IsSuccess = true;
                ViewBag.Result = "Có lỗi xảy ra. Vui lòng liên hệ admin.";
            }

            return View();
        }

        [Route("order/xac-nhan-huy-don-hang.html")]
        public ActionResult CancelOrder()
        {
            return View();
        }

        [Route("order-history.html")]
        public ActionResult History()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                var getCustommer = _orderBusiness.GetListByCustomerId(user.Id);
                var order = new List<HistoryViewModel>();

                var ordervm = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(getCustommer);
                foreach (var item in ordervm)
                {
                    var getdetail = _orderBusiness.GetListDetailById(item.OrderID).ToList();
                    var product = _productBusiness.GetById(getdetail.ElementAt(0).ProductID);
                    ViewBag.totail = (getdetail.ElementAt(0).Price * getdetail.ElementAt(0).Quantity).ToString("#,##");
                    ViewBag.Name = product.Name;
                }

                return View(ordervm);
            }

            return View();
        }
    }
}