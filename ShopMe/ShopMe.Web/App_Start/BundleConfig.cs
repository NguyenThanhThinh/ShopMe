using System.Web.Optimization;

namespace ShopMe.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Script APP
            const string SCRIPT_APP = "~/Scripts/app/";
            var scriptBundle_APP = new ScriptBundle("~/bundles/AppScript").Include(
                SCRIPT_APP + "logout.js",
                SCRIPT_APP + "app.js",
                SCRIPT_APP + "common.js",
                SCRIPT_APP + "search.js",
                SCRIPT_APP + "commoncart.js",
                SCRIPT_APP + "shoppingcart.js"
            );
            bundles.Add(scriptBundle_APP);

            const string SCRIPT_APPCheckout = "~/Scripts/app/";
            var SCRIPT_APPCk = new ScriptBundle("~/bundles/AppScriptCheckout").Include(
                SCRIPT_APPCheckout + "commoncart.js",
                SCRIPT_APPCheckout + "shoppingcart.js"
            );
            bundles.Add(SCRIPT_APPCk);
            //script UI
            const string SCRIPT_CLIENT = "~/Content/js/";
            var scriptBundle_Client = new ScriptBundle("~/bundles/CommonClient").Include(
                SCRIPT_CLIENT + "jquery-2.1.1.min.js",
                SCRIPT_CLIENT + "mustache.min.js",
                SCRIPT_CLIENT + "numeral.min.js",
                SCRIPT_CLIENT + "bootbox.js",
                SCRIPT_CLIENT + "toastr.js",
                SCRIPT_CLIENT + "jquery-ui.js",
                SCRIPT_CLIENT + "bootstrap.min.js",
                SCRIPT_CLIENT + "velocity.min.js",
                SCRIPT_CLIENT + "jquery.waypoints.min.js",
                SCRIPT_CLIENT + "queryloader2.min.js",
                SCRIPT_CLIENT + "rs-plugin/js/jquery.themepunch.tools.min.js",
                SCRIPT_CLIENT + "rs-plugin/js/jquery.themepunch.tools.min.js",
                SCRIPT_CLIENT + "rs-plugin/js/jquery.themepunch.revolution.min.js",
                SCRIPT_CLIENT + "jquery.appear.js",
                SCRIPT_CLIENT + "owlcarousel/owl.carousel.min.js",
                SCRIPT_CLIENT + "jquery.countdown.plugin.min.js",
                SCRIPT_CLIENT + "jquery.countdown.min.js",
                SCRIPT_CLIENT + "arcticmodal/jquery.arcticmodal.js",
                SCRIPT_CLIENT + "colorpicker/colorpicker.js",
                SCRIPT_CLIENT + "retina.min.js",
                SCRIPT_CLIENT + "theme.plugins.js",
                SCRIPT_CLIENT + "theme.core.js"
            );
            bundles.Add(scriptBundle_Client);


            //Scripts Shared
            const string ANGULAR_APP_ROOT_SHARED = "~/spa/shared/";
            const string VIRTUAL_BUNDLE_PATH_SHARED = ANGULAR_APP_ROOT_SHARED + "js/shared.js";
            var scriptBundle_Shared = new ScriptBundle(VIRTUAL_BUNDLE_PATH_SHARED).Include(
                ANGULAR_APP_ROOT_SHARED + "modules/shopme.common.js",
                ANGULAR_APP_ROOT_SHARED + "filter/isPublishFilter.js",
                ANGULAR_APP_ROOT_SHARED + "filter/isStatusOrderFilter.js",
                ANGULAR_APP_ROOT_SHARED + "services/apiService.js",
                ANGULAR_APP_ROOT_SHARED + "directives/pagerDirective.js",
                ANGULAR_APP_ROOT_SHARED + "directives/asDateDirective.js",
                ANGULAR_APP_ROOT_SHARED + "services/notificationService.js",
                ANGULAR_APP_ROOT_SHARED + "services/authData.js",
                ANGULAR_APP_ROOT_SHARED + "services/authenticationService.js",
                ANGULAR_APP_ROOT_SHARED + "services/loginService.js",
                ANGULAR_APP_ROOT_SHARED + "services/commonService.js"
            );
            bundles.Add(scriptBundle_Shared);

            //Scripts Module

            const string ANGULAR_APP_ROOT_APP = "~/spa/components/";

            const string VIRTUAL_BUNDLE_PATH_MODULE = ANGULAR_APP_ROOT_APP + "js/module.js";
            var scriptBundle_Module = new ScriptBundle(VIRTUAL_BUNDLE_PATH_MODULE)
                .IncludeDirectory(ANGULAR_APP_ROOT_APP, "*.module.js", searchSubdirectories: true);
            bundles.Add(scriptBundle_Module);


            //Scripts controller                                           
            const string VIRTUAL_BUNDLE_PATH_CONTROLLER = ANGULAR_APP_ROOT_APP + "js/controller.js";
            var scriptBundle_Controller = new ScriptBundle(VIRTUAL_BUNDLE_PATH_CONTROLLER)
                .IncludeDirectory(ANGULAR_APP_ROOT_APP, "*Controller.js", searchSubdirectories: true);
            bundles.Add(scriptBundle_Controller);

            bundles.Add(new StyleBundle("~/bundles/thanhthinh").Include(
                "~/Content/css/style.css"
            ));

            //Style CSS
            //const string STYLE_CSS = "~/Content/css/";
            //var StyleBundle_APP = new StyleBundle("~/bundles/thanhthinh").Include(
            //      STYLE_CSS + "animate.css",
            //      STYLE_CSS + "fontello.css",
            //      STYLE_CSS + "bootstrap.min.css",
            //      STYLE_CSS + "jquery-ui.min.css",
            //      STYLE_CSS + "toastr.min.css",
            //      STYLE_CSS + "settings.css",
            //       STYLE_CSS + "owl.carousel.css",
            //      STYLE_CSS + "jquery.arcticmodal.css",
            //      STYLE_CSS + "toastr.min.css",
            //      STYLE_CSS + "settings.css",
            //      STYLE_CSS+ "comom.css",
            //           STYLE_CSS + "syle.css",
            //      STYLE_CSS + "spacing.min.css",
            //      STYLE_CSS + "PagedList.css"
            //        );
            //bundles.Add(StyleBundle_APP);
        }
    }
}