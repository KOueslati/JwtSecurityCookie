using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

namespace WebApi
{
    public partial class Startup
    {
        public void ConfigureWebApi(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "WebApiApplication",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
        }
    }
}