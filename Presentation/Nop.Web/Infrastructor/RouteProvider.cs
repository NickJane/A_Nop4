using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework;

namespace Nop.Web.Infrastructor
{
    public class RouteProvider : Framework.Routes.IRouteProvider
    {
        public int Priority { get { return 1; } }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "DefaultIndex",
                url: "index",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , namespaces: new string[] { "Nop.Web.Controllers" }
            );
        }
    }
}