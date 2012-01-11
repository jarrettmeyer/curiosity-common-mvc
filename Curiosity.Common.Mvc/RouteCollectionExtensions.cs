using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Curiosity.Common.Mvc
{
    public static class RouteCollectionExtensions
    {
        public static Route MapLowercaseRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.MapLowercaseRoute(name, url, defaults, null);
        }

        public static Route MapLowercaseRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            if (routes == null)            
                throw new ArgumentNullException("routes");            
            if (url == null)
                throw new ArgumentNullException("url");
            

            Route route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary(),
            };

            routes.Add(name, route);

            return route;
        }

        public static Route MapLowercaseRoute(this AreaRegistrationContext context, string name, string url, object defaults)
        {
            return context.MapLowercaseRoute(name, url, defaults, null);
        }

        public static Route MapLowercaseRoute(this AreaRegistrationContext context, string name, string url, object defaults, object constraints)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (url == null)
                throw new ArgumentNullException("url");


            Route route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary(),
            };

            context.Routes.Add(route);
            route.DataTokens["area"] = context.AreaName;
            route.DataTokens["UseNamespaceFallback"] = true;

            return route;
        }
    }
}
