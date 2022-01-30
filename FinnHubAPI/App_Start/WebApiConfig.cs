using FinnHubAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace FinnHubAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Config.SQLConnectionString = ConfigurationManager.ConnectionStrings["SQLServer"].ToString();

            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
