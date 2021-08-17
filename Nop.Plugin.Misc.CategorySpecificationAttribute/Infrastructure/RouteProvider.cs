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
            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttributeGroup.Create",
                "Admin/CategorySpecificationAttributeGroup/Create",
                new { controller = "CategorySpecificationAttributeGroup", action = "Create" });

            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttributeGroup.GetSpecificationAttributesByCategoryId",
                "Plugins/CategorySpecificationAttributeGroup/GetSpecificationAttributesByCategoryId",
                new { controller = "CategorySpecificationAttributeGroup", action = "GetSpecificationAttributesByCategoryId" });

            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttributeGroup.Product.ProductSpecificationAttributeAdd",
                "Plugins/CategorySpecificationAttributeGroup/Product/ProductSpecificationAttributeAdd",
                new { controller = "Product", action = "ProductSpecificationAttributeAdd" });

            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttributeGroup.Product.Create",
                "Plugins/CategorySpecificationAttributeGroup/Product/Create",
                new { controller = "Product", action = "Create" });

            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttributeGroup.Product.ProductPictureAdd",
                "Plugins/CategorySpecificationAttributeGroup/Product/ProductPictureAdd",
                new { controller = "Product", action = "ProductPictureAdd" });

            endpointRouteBuilder.MapControllerRoute("Plugin.Misc.CategorySpecificationAttributeGroup.Picture.Upload",
                "Plugins/CategorySpecificationAttributeGroup/Picture/Upload",
                new { controller = "Picture", action = "Upload" });
        }
    }
}
