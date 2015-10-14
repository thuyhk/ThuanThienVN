using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HGT.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region rilk: routing admin
            #region Common area

            var commonDefaults = new RouteValueDictionary();
            commonDefaults.Add("controller", "login");
            commonDefaults.Add("action", "Index");
            commonDefaults.Add("id", UrlParameter.Optional);

            var commonContrains = new RouteValueDictionary();

            var commonTokens = new RouteValueDictionary();
            commonTokens.Add("Namespaces", new string[] { typeof(HGT.Admin.Controllers.LoginController).Namespace });
            commonTokens.Add("area", "Common");

            routes.Add("Common_Default",
                       new Route("common/{controller}/{action}/{id}",
                                 commonDefaults,
                                 commonContrains,
                                 commonTokens,
                                 new MvcRouteHandler()));

            #endregion

            #region Admin area

            var adminDefaults = new RouteValueDictionary();
            adminDefaults.Add("controller", "Dashboard");
            adminDefaults.Add("action", "Index");
            adminDefaults.Add("id", UrlParameter.Optional);

            var adminContrains = new RouteValueDictionary();

            var adminTokens = new RouteValueDictionary();
            adminTokens.Add("Namespaces", new string[] { typeof(HGT.Admin.Controllers.DashboardController).Namespace });
            adminTokens.Add("area", "Admin");

            routes.Add("Admin_Default",
                       new Route("admin/{controller}/{action}/{id}/",
                                 adminDefaults,
                                 adminContrains,
                                 adminTokens,
                                 new MvcRouteHandler()));


            #endregion

            #endregion

            #region front end

            //routes.MapMvcAttributeRoutes(); //Enabling attribute based routing
            // home
            routes.MapRoute("Home",
                            "",
                            new { controller = "Home", action = "Index" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });

            // refresh captcha
            routes.MapRoute("GenerateCaptcha",
                            "GenerateCaptcha",
                            new { controller = "Home", action = "GenerateCaptcha" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // contact
            routes.MapRoute("Introduction",
                            "gioi-thieu",
                            new { controller = "Home", action = "Introduction" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });

            // contact
            routes.MapRoute("Contact",
                            "lien-he",
                            new { controller = "Home", action = "Contact" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });

            // about us
            routes.MapRoute("AboutUs",
                            "thong-tin/{Alias}",
                            new { controller = "News", action = "Detail", Alias = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // new
            routes.MapRoute("ListSupport",
                            "tu-van-ho-tro",
                            new { controller = "News", action = "ListSupport" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // about us
            routes.MapRoute("NewsDetail",
                            "chi-tiet/{alias}",
                            new { controller = "News", action = "Detail", alias = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // store
            routes.MapRoute("Store",
                            "cua-hang",
                            new { controller = "Common", action = "ListStores" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // store detail
            routes.MapRoute("StoreDetail",
                            "cua-hang/{Alias}",
                            new { controller = "Common", action = "StoreDetail", Alias = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // service
            routes.MapRoute("Solution",
                            "giai-phap",
                            new { controller = "Home", action = "ServicePage" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // service detail
            routes.MapRoute("SolutionDetail",
                            "giai-phap/{Alias}",
                            new { controller = "Home", action = "SolutionDetail", Alias = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // quick search
            routes.MapRoute("QuickSearch",
                            "tim-kiem/{key}",
                            new { controller = "Category", action = "QuickSearch", key = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            #region #Cart

            // shopping cart
            routes.MapRoute("ShoppingCart",
                            "gio-hang",
                            new { controller = "ShoppingCart", action = "index" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // add to cart
            routes.MapRoute("AddToCart",
                            "ShoppingCart/AddToCart/{id}",
                            new { controller = "ShoppingCart", action = "AddToCart", id = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // add to quick cart
            routes.MapRoute("AddToQuickCart",
                            "ShoppingCart/AddToQuickCart/{id}",
                            new { controller = "ShoppingCart", action = "AddToQuickCart", id = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // remove from cart
            routes.MapRoute("RemoveFromCart",
                            "ShoppingCart/RemoveFromCart/{id}",
                            new { controller = "ShoppingCart", action = "RemoveFromCart", id = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // update cart
            routes.MapRoute("UpdateCart",
                            "ShoppingCart/UpdateCart/{id}/{quantity}",
                            new { controller = "ShoppingCart", action = "UpdateCart", id = UrlParameter.Optional, quantity = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });

            // show quick cart
            routes.MapRoute("ShowQuickCart",
                            "ShoppingCart/_ShowQuickCart",
                            new { controller = "ShoppingCart", action = "_ShowQuickCart" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });

            // show mini cart
            routes.MapRoute("QuickCart",
                            "ShoppingCart/_QuickCart",
                            new { controller = "ShoppingCart", action = "_QuickCart" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });
            // create order
            routes.MapRoute("CreateOrder",
                            "Checkout/_CheckoutInFormCart",
                            new { controller = "Checkout", action = "_CheckoutInFormCart" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });

            #endregion

            // all categories
            routes.MapRoute("ListAllCategories",
                            "san-pham",
                            new { controller = "Category", action = "Index" },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });

            // category detail
            routes.MapRoute("CategoryDetail",
                            "{categoryAlias}",
                            new { controller = "Category", action = "CategoryDetail", categoryAlias = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });

            // product detail
            routes.MapRoute("ProductDetail",
                            "{categoryAlias}/{productAlias}",
                            new { controller = "Category", action = "ProductDetail", categoryAlias = UrlParameter.Optional, productAlias = UrlParameter.Optional },
                            new[] { typeof(HGT.Web.Controllers.HomeController).Namespace });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { typeof(HGT.Web.Controllers.HomeController).Namespace } // Parameter defaults
            );


            #endregion
        }
    }
}
