using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.CategorySpecificationAttribute.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority => 0;

        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttribute.Create",
                "Plugins/CategorySpecificationAttribute/Create",
                new { controller = "CategorySpecificationAttribute", action = "Create" });

            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttribute.GetSpecificationAttributesByCategoryId",
                "Plugins/CategorySpecificationAttribute/GetSpecificationAttributesByCategoryId",
                new { controller = "CategorySpecificationAttribute", action = "GetSpecificationAttributesByCategoryId" });

            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttribute.ProductSpecificationAttributeAdd",
                "Plugins/CategorySpecificationAttribute/ProductSpecificationAttributeAdd",
                new { controller = "CategorySpecificationAttribute", action = "ProductSpecificationAttributeAdd" });

            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttribute.CreateProduct",
                "Plugins/CategorySpecificationAttribute/CreateProduct",
                new { controller = "CategorySpecificationAttribute", action = "CreateProduct" });
        }
    }
}
