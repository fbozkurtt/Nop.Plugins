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
    [ViewComponent(Name = AdvancedSpecificationAttributesDefaults.CUSTOM_SPECIFICATION_ATTRIBUTE_VIEW_COMPONENT_NAME)]
    public class CustomSpecificationAttributeViewComponent : NopViewComponent
    {
        #region Fields

        private readonly ICustomSpecificationAttributeModelFactory _customSpecificationAttributeModelFactory;
        private readonly ICustomSpecificationAttributeService _customSpecificationAttributeService;

        #endregion

        #region Ctor
        public CustomSpecificationAttributeViewComponent(ICustomSpecificationAttributeModelFactory customSpecificationAttributeModelFactory, ICustomSpecificationAttributeService customSpecificationAttributeService)
        {
            _customSpecificationAttributeModelFactory = customSpecificationAttributeModelFactory;
            _customSpecificationAttributeService = customSpecificationAttributeService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {

            if (additionalData is not SpecificationAttributeModel model)
                return Content(string.Empty);

            //if (model.Id == 0)
            //    return Content(string.Empty);

            var customSpesificationAttribute = await _customSpecificationAttributeService.GetBySpecificationAttributeIdAsync(model.Id);

            var customSpecificationAttributeModel = customSpesificationAttribute == null
                ? await _customSpecificationAttributeModelFactory
              .PrepareCustomSpecificationAttributeModelAsync(new CustomSpecificationAttributeModel(), null)
              : await _customSpecificationAttributeModelFactory
                .PrepareCustomSpecificationAttributeModelAsync(null, customSpesificationAttribute);

            return View("~/Plugins/Misc.AdvancedSpecificationAttributes/Views/SpecificationAttribute/_CreateOrUpdateSpecificationAttribute.Plugin.cshtml", customSpecificationAttributeModel);
        }
    }
}
