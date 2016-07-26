using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HanzoProxyAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "test",
                routeTemplate: "api/vm/test",
                defaults: new { controller = "VirtualMachine", action = "DoTest" }
            );
            config.Routes.MapHttpRoute(
                name: "VMListAll",
                routeTemplate: "api/vm/list",
                defaults: new { controller = "VirtualMachine", action = "GetListAll" }
            );
            config.Routes.MapHttpRoute(
                name: "VMListByResource",
                routeTemplate: "api/vm/list/resource/{resource}",
                defaults: new { controller = "VirtualMachine", action = "GetListByResource" }
            );
            config.Routes.MapHttpRoute(
                name: "VMManage",
                routeTemplate: "api/vm/manage",
                defaults: new { controller = "VirtualMachine", action = "PostManage" }
            );
            config.Routes.MapHttpRoute(
                name: "VMSchedule",
                routeTemplate: "api/vm/schedule",
                defaults: new { controller = "VirtualMachine", action = "PostSchedule" }
            );
        }
    }
}
