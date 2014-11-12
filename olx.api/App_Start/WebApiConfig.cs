using System.Net.Http.Formatting;
using System.Web.Http;

namespace olx.api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
         
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}"
            );
            config.Routes.MapHttpRoute(
                name: "All Ads",
                routeTemplate: "api/{controller}/all",
                defaults: new { action = "All" }
            );
            config.Routes.MapHttpRoute(
                name: "Images of an Ad",
                routeTemplate: "api/{controller}/images/{uniqueId}",
                defaults: new { action = "Images" }
            );
            config.Routes.MapHttpRoute(
                name: "Ad Images",
                routeTemplate: "api/{controller}/images",
                defaults: new { action = "Images" }
            );
            config.Routes.MapHttpRoute(
                name: "Ad Get",
                routeTemplate: "api/{controller}/get",
                defaults: new { action = "Get" }
            );
            config.Routes.MapHttpRoute(
                name: "Ad Count",
                routeTemplate: "api/{controller}/count",
                defaults: new { action = "Count" }
            );
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            config.EnableSystemDiagnosticsTracing();
        }
    }
}
