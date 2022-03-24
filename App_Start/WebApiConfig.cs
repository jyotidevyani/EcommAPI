using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Unity;
using EcommerceAPI.App_Start;
using EcommerceAPI.Contract;
using EcommerceAPI.Repository;

using System.Web.Http.Filters;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;

namespace EcommerceAPI
{
    public static class WebApiConfig
    {
        //public static void Register(HttpConfiguration config)
        //{
        //    // Web API configuration and services

        //    // Web API routes
        //    config.MapHttpAttributeRoutes();

        //    config.Routes.MapHttpRoute(
        //        name: "DefaultApi",
        //        routeTemplate: "api/{controller}/{id}",
        //        defaults: new { id = RouteParameter.Optional }
        //    );
        //}

        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new ValidationActionFilter());
            // Web API configuration and services
            IUnityContainer container = new UnityContainer();
            //Register dependency
            container.RegisterType<iProductService, clsProduct>();
            // Web API routes
            config.MapHttpAttributeRoutes();
            //Inject action filters 
            var providers = config.Services.GetFilterProviders().ToList();
            var defaultprovider = providers.Single(i => i is ActionDescriptorFilterProvider);
            config.Services.Remove(typeof(IFilterProvider), defaultprovider);
            config.Services.Add(typeof(IFilterProvider), new UnityFilterProvider(container));

            //Resolve dependency
            config.DependencyResolver = new UnityResolver(container);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.EnableCors();
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
        }

    }
}
