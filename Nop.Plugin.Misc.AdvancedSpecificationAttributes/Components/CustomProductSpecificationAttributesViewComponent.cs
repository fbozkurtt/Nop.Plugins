using Microsoft.AspNetCore.Mvc;
using Nop.Data;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Factories;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Components
{
    [ViewComponent(Name = AdvancedSpecificationAttributesDefaults.CUSTOM_PRODUCT_SPECIFICATION_ATTRIBUTES_VIEW_COMPONENT_NAME)]
    public class CustomProductSpecificationAttributesViewComponent : NopViewComponent
    {
        #region Fields

        private readonly ICustomSpecificationAttributeModelFactory _customSpecificationAttributeModelFactory;
        private readonly ICustomSpecificationAttributeService _customSpecificationAttributeService;

        #endregion

        #region Ctor
        public CustomProductSpecificationAttributesViewComponent(ICustomSpecificationAttributeModelFactory customSpecificationAttributeModelFactory, ICustomSpecificationAttributeService customSpecificationAttributeService)
        {
            _customSpecificationAttributeModelFactory = customSpecificationAttributeModelFactory;
            _customSpecificationAttributeService = customSpecificationAttributeService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {

            if (additionalData is not ProductModel model)
                return Content(string.Empty);

            return View("~/Plugins/Misc.AdvancedSpecificationAttributes/Views/Product/_CreateOrUpdate.Plugin.cshtml", model);
        }
    }
}
