using Microsoft.AspNetCore.Mvc.Rendering;

namespace DncyTemplate.Mvc.HtmlHelpers
{
    public static class MenuActivteHelper
    {
        public static string IsMenuActive(this IHtmlHelper htmlHelper, string controller, string action, string area = null)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            // var areaName = routeData.Values["area"]?.ToString();
            var routeAction = routeData.Values["action"]?.ToString();
            var routeController = routeData.Values["controller"]?.ToString();
            var returnActive = (controller == routeController && action == routeAction);
            return returnActive ? "active" : "";
        }
    }
}