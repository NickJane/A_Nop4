using System.Web.Routing;

namespace Nop.Web.Framework.Routes
{
    public interface IRouteProvider
    {
        void RegisterRoutes(RouteCollection routes);

        int Priority { get; }
    }
}