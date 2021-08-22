using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services;
using Nop.Services.Catalog;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes
{
    /// <summary>
    /// Rename this file and change to the correct type
    /// </summary>
    public class AdvancedSpecificationAttributesPlugin : BasePlugin, IWidgetPlugin
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly ICustomSpecificationAttributeService _customSpecificationAttributeService;

        #endregion

        #region Ctor

        public AdvancedSpecificationAttributesPlugin(ILocalizationService localizationService, ISpecificationAttributeService specificationAttributeService, ICustomSpecificationAttributeService customSpecificationAttributeService)
        {
            _localizationService = localizationService;
            _specificationAttributeService = specificationAttributeService;
            _customSpecificationAttributeService = customSpecificationAttributeService;
        }

        #endregion

        #region Properties

        public bool HideInWidgetList => false;

        #endregion

        #region Methods

        public string GetWidgetViewComponentName(string widgetZone)
        {
            if (widgetZone.Equals(AdminWidgetZones.SpecificationAttributeGroupDetailsBlock))
                return "CategorySpecificationAttributeGroup";

            if (widgetZone.Equals(AdminWidgetZones.SpecificationAttributeDetailsBlock))
                return AdvancedSpecificationAttributesDefaults.CUSTOM_SPECIFICATION_ATTRIBUTE_VIEW_COMPONENT_NAME;

            if (widgetZone.Equals(PublicWidgetZones.SearchBoxBeforeSearchButton))
                return "CreateProductButton";

            return string.Empty;
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string>
            {
                AdminWidgetZones.SpecificationAttributeGroupDetailsBlock,
                AdminWidgetZones.SpecificationAttributeDetailsBlock,
                PublicWidgetZones.SearchBoxBeforeSearchButton
            });
        }

        public override async Task InstallAsync()
        {
            ////locales
            //await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            //{
            //    ["Plugins.Misc.AdvancedSpecificationAttributes.CustomFields.CustomerRole"] = "Required customer role",
            //    ["Plugins.DiscountRules.CustomerRoles.Fields.CustomerRole.Hint"] = "Discount will be applied if customer is in the selected customer role.",
            //    ["Plugins.DiscountRules.CustomerRoles.Fields.CustomerRole.Select"] = "Select customer role",
            //    ["Plugins.DiscountRules.CustomerRoles.Fields.CustomerRoleId.Required"] = "Customer role is required",
            //    ["Plugins.DiscountRules.CustomerRoles.Fields.DiscountId.Required"] = "Discount is required"
            //});

            foreach (var specificationAttribute in (await _specificationAttributeService.GetSpecificationAttributesAsync()))
            {
                await _customSpecificationAttributeService.InsertCustomSpecificationAttributeAsync(new CustomSpecificationAttribute()
                {
                    SpecificationAttributeId = specificationAttribute.Id,
                    IsRequired = false,
                    AttributeControlType = AttributeControlType.DropdownList,
                    AttributeFilterType = AttributeFilterType.Checkboxes
                });
            }

            await base.InstallAsync();
        }

        #endregion
    }
}
