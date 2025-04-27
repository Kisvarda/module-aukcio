using DotNetNuke.Web.Mvc.Routing;


namespace Kisvarda.Dnn.Dnn.Kisvarda.Aukcio
{
    public class RouteMapper : IMvcRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapRoute(
                "Aukcio",
                "Aukcio",
                "{controller}/{action}",
                new[] { "Kisvarda.Dnn.Dnn.Kisvarda.Aukcio.Controllers" });
        }
    }
}