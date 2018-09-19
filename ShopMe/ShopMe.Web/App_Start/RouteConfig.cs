using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopMe.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            //routes.MapRoute(
            //             name: "Search",
            //             url: "tim-kiem.html",
            //             defaults: new { controller = "Product", action = "Search", productId = UrlParameter.Optional }

            //         );
            //routes.MapRoute(
            //     name: "Login",
            //     url: "dang-nhap.html",
            //     defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            //     namespaces: new string[] { "ShopMe.Web.Controllers" }
            // );

            // routes.MapRoute(
            //    name: "Checkout",
            //    url: "order-history.html",
            //    defaults: new { controller = "ShoppingCart", action = "History", id = UrlParameter.Optional },
            //    namespaces: new string[] { "ShopMe.Web.Controllers" }
            //);

            //    routes.MapRoute(
            //    name: "order",
            //    url: "order.html",
            //    defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new string[] { "ShopMe.Web.Controllers" }
            //);
            //     routes.MapRoute(
            //    name: "history",
            //    url: "order/checkout.html",
            //    defaults: new { controller = "ShoppingCart", action = "Checkout", id = UrlParameter.Optional },
            //    namespaces: new string[] { "ShopMe.Web.Controllers" }
            //);
            routes.MapRoute(
              name: "Index",
              url: "trang-chu.html",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
              namespaces: new string[] { "ShopMe.Web.Controllers" }
          );
            // routes.MapRoute(
            //    name: "Register",
            //    url: "dang-ky.html",
            //    defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
            //    namespaces: new string[] { "ShopMe.Web.Controllers" }
            //);
            //routes.MapRoute(
            //                name: "Product",
            //                url: "{Alias}.sp-{ProductID}.html",
            //                defaults: new { controller = "Product", action = "Detail", productId = UrlParameter.Optional }

            //            );

            //routes.MapRoute(
            //    name: "Product Category",
            //    url: "{alias}.pc-{id}.html",
            //    defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
            //      namespaces: new string[] { "ShopMe.Web.Controllers" }
            //);

            //     routes.MapRoute(
            //    name: "Login",
            //    url: "dang-nhap.html",
            //    defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            //    namespaces: new string[] { "ShopMe.Web.Controllers" }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ShopMe.Web.Controllers" }
            );




        }
    }
}
