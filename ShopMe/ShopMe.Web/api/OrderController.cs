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
using System.Web.Script.Serialization;

namespace ShopMe.Web.api
{
    [RoutePrefix("api/order")]
    [Authorize]
    public class OrderController : ApiControllerBase
    {
        private IOrderBusiness _orderService;
        private ICommonBusiness _commonService;
        private IProductBusiness _productService;

        public OrderController(IErrorBusiness errorService, IOrderBusiness orderService,
            IProductBusiness productService, ICommonBusiness commonService) : base(errorService)
        {
            _orderService = orderService;
            _productService = productService;
            _commonService = commonService;
        }

        [Route("getall")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20,
            int filterStatus = 0)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _orderService.GetAll(keyword);
                if (filterStatus != 0)
                {
                    model = model.Where(x => x.Status == filterStatus);
                }

                model = model.OrderByDescending(x => x.OrderID);
                totalRow = model.Count();
                var query = (pageSize == -1)
                    ? model.OrderBy(x => x.CreatedDate)
                    : model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(query);
                var paginationSet = new PaginationSet<OrderViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (pageSize == -1) ? 1 : (int) Math.Ceiling((decimal) totalRow / pageSize)
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
                var order = _orderService.GetById(id);
                var responseData = Mapper.Map<Order, OrderViewModel>(order);
                if (order == null)
                {
                    return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không tìm thấy!");
                }

                var listDetails = _orderService.GetListDetailById(order.OrderID);
                responseData.OrderDetail =
                    Mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(listDetails);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getByEmail")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetByEmail(HttpRequestMessage request, string email)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetListByEmail(email).OrderByDescending(x => x.CreatedDate);
                var responseData = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getbycustomer")]
        [HttpGet]
        public HttpResponseMessage GetByCustomerId(HttpRequestMessage request, string customerId)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetListByCustomerId(customerId).OrderByDescending(x => x.CreatedDate);
                var responseData = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderShowViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getbyemailandid")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetByEmailAndId(HttpRequestMessage request, string email, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetByEmailAndId(email, id);
                var responseData = Mapper.Map<Order, OrderShowViewModel>(model);
                if (model == null)
                {
                    return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không tìm thấy!");
                }

                var listDetails = _orderService.GetListDetailById(model.OrderID);
                responseData.OrderDetails =
                    Mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailShowViewModel>>(listDetails);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("getnumberordernew")]
        [HttpGet]
        [Authorize(Roles = "Access")]
        public HttpResponseMessage GetNumberOrderNew(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetNumberOrderNew();
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getlatestorder")]
        [HttpGet]
        public HttpResponseMessage GetLatestOrder(HttpRequestMessage request, int top)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetLatestOrder(top);
                var responseData = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }


        [Route("update")]
        [HttpPut]
        [Authorize(Roles = "Edit")]
        public HttpResponseMessage Update(HttpRequestMessage request, OrderViewModel orderVm)
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
                    try
                    {
                        var dbOrder = _orderService.GetById(orderVm.OrderID);
                        var ordetail = _orderService.GetByDetail(orderVm.OrderID);
                        dbOrder.UpdateOrder(orderVm);
                        _orderService.Update(dbOrder);
                        _orderService.Save();
                        bool issellproduct = true;
                        if (orderVm.Status.Value == 40)
                        {
                            issellproduct = _productService.SellProduct(ordetail.ProductID, ordetail.Quantity);
                        }

                        var responseData = Mapper.Map<Order, OrderViewModel>(dbOrder);


                        _productService.Save();
                        response = request.CreateResponse(HttpStatusCode.Created, responseData);
                    }
                    catch (Exception ex)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                    }
                }

                return response;
            });
        }

        [Route("getlistproductbyorderid")]
        [HttpGet]
        public HttpResponseMessage GetListProductByOrderId(HttpRequestMessage request, int id, int page,
            int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var listOrderDetail = _orderService.GetListDetailById(id);
                totalRow = listOrderDetail.Count();
                var query = listOrderDetail.OrderBy(x => x.ProductID).Skip(page * pageSize).Take(pageSize);
                var responseData = Mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(query);
                var paginationSet = new PaginationSet<OrderDetailViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int) Math.Ceiling((decimal) totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("orderstatus")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetOrderStatus(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = Domains.Common.EnumHelper.ToDictionary(typeof(OrderStatus))
                    .Select(x => new {Id = x.Key, Name = x.Value});
                if (model == null)
                {
                    return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không tìm thấy!");
                }

                var listConfig = new List<SystemConfig>();
                foreach (var item in model)
                {
                    listConfig.Add(_commonService.GetSystemConfig(item.Name));
                }

                var responseData =
                    Mapper.Map<IEnumerable<SystemConfig>, IEnumerable<SystemConfigViewModel>>(listConfig);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("paymentstatus")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetPaymentStatus(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = Domains.Common.EnumHelper.ToDictionary(typeof(PaymentStatus))
                    .Select(x => new {Id = x.Key, Name = x.Value});
                if (model == null)
                {
                    return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không tìm thấy!");
                }

                var listConfig = new List<SystemConfig>();
                foreach (var item in model)
                {
                    listConfig.Add(_commonService.GetSystemConfig(item.Name));
                }

                var responseData =
                    Mapper.Map<IEnumerable<SystemConfig>, IEnumerable<SystemConfigViewModel>>(listConfig);
                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, string orderViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                ShoppingCartAdmin orderVm = new JavaScriptSerializer().Deserialize<ShoppingCartAdmin>(orderViewModel);
                Validate<ShoppingCartAdmin>(orderVm);
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    try
                    {
                        var newOrder = new Order();
                        newOrder.UpdateOrderCart(orderVm);
                        newOrder.CreatedDate = DateTime.Now;
                        newOrder.Status = _commonService.GetSystemConfig("Pending").ValueInt.Value;
                        newOrder.PaymentStatus = _commonService.GetSystemConfig("Unpaid").ValueInt.Value;

                        List<OrderDetail> orderDetails = new List<OrderDetail>();
                        bool isEnough = true;
                        foreach (var item in orderVm.OrderDetails)
                        {
                            var detail = new OrderDetail();
                            detail.ProductID = item.ProductID;
                            detail.Quantity = item.Quantity;

                            detail.Price = item.Price;
                            orderDetails.Add(detail);
                            //Check product quantity
                            isEnough = _productService.SellProduct(item.ProductID, item.Quantity);
                            if (isEnough == false)
                            {
                                return request.CreateErrorResponse(HttpStatusCode.BadRequest,
                                    "Sản phẩm " + _productService.GetById(item.ProductID).Name + " đã hết hàng!");
                            }
                        }

                        _orderService.Create(ref newOrder, orderDetails);
                        _orderService.Save();

                        //Sendmail order info
                        string content = System.IO.File.ReadAllText(
                            System.Web.Hosting.HostingEnvironment.MapPath("~/Assets/admin/templates/order_reply.html"));
                        content = content.Replace("{{Name}}", orderVm.CustomerName);
                        content = content.Replace("{{Email}}", orderVm.CustomerEmail);
                        content = content.Replace("{{OrderID}}", newOrder.OrderID.ToString());
                        MailHelper.SendMail(orderVm.CustomerEmail, "Thông tin đơn hàng từ ShopMe", content);
                        var responseData = Mapper.Map<Order, OrderViewModel>(newOrder);
                        response = request.CreateResponse(HttpStatusCode.Created, responseData);
                    }
                    catch (Exception ex)
                    {
                        return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                    }
                }

                return response;
            });
        }

        [Route("countnew")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage CountOrderNew(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _orderService.GetNumberOrderNew();
                var response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }
    }
}