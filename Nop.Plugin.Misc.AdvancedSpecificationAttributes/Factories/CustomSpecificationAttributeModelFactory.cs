using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Extensions;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Factories
{
    public class CustomSpecificationAttributeModelFactory : ICustomSpecificationAttributeModelFactory
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly ICustomSpecificationAttributeParser _customSpecificationAttributeParser;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly ICustomSpecificationAttributeService _customSpecificationAttributeService;

        #endregion

        #region Ctor

        public CustomSpecificationAttributeModelFactory(
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            ICustomSpecificationAttributeParser customSpecificationAttributeParser,
            ISpecificationAttributeService specificationAttributeService,
            ICustomSpecificationAttributeService customSpecificationAttributeService)
        {
            _localizationService = localizationService;
            _localizedModelFactory = localizedModelFactory;
            _customSpecificationAttributeParser = customSpecificationAttributeParser;
            _specificationAttributeService = specificationAttributeService;
            _customSpecificationAttributeService = customSpecificationAttributeService;
        }

        #endregion

        #region Utilities

        protected async Task PrepareConditionAttributesModelAsync(ConditionModel model, CustomSpecificationAttribute customSpecificationAttribute)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (customSpecificationAttribute == null)
                throw new ArgumentNullException(nameof(customSpecificationAttribute));

            model.EnableCondition = !string.IsNullOrEmpty(customSpecificationAttribute.ConditionAttributeXml);

            //get selected specification attribute
            var selectedAttribute = (await _customSpecificationAttributeParser.ParseCustomSpecificationAttributesAsync(customSpecificationAttribute.ConditionAttributeXml)).FirstOrDefault();
            model.SelectedAttributeId = selectedAttribute?.Id ?? 0;

            //get selected specification attribute options identifiers
            var selectedValuesIds = await _customSpecificationAttributeParser
                .ParseSpecificationAttributeOptions(customSpecificationAttribute.ConditionAttributeXml).SelectMany(ta => ta.options.Select(v => v.Id)).ToListAsync();

            //get available condition specification attributes (ignore this attribute and non-combinable attributes)
            var availableConditionAttributes = (await _customSpecificationAttributeService.GetAllCustomSpecificationAttributesAsync())
                .Where(attribute => attribute.Id != customSpecificationAttribute.Id && attribute.CanBeUsedAsCondition());

            model.ConditionAttributes = await availableConditionAttributes.SelectAwait(async attribute => new AttributeConditionModel
            {
                Id = attribute.Id,
                Name = (await _specificationAttributeService.GetSpecificationAttributeByIdAsync(attribute.SpecificationAttributeId)).Name,
                AttributeControlType = attribute.AttributeControlType,
                Options = await (await _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttributeAsync(attribute.SpecificationAttributeId)).Select(option => new SelectListItem
                {
                    Text = option.Name,
                    Value = option.Id.ToString(),
                    Selected = selectedAttribute?.Id == attribute.Id && selectedValuesIds.Contains(option.Id)
                }).ToListAsync()
            }).ToListAsync();
        }

        #endregion

        #region Methods
        public async Task<CustomSpecificationAttributeModel> PrepareCustomSpecificationAttributeModelAsync(CustomSpecificationAttributeModel model, CustomSpecificationAttribute customSpecificatonAttribute, bool excludeProperties = false)
        {
            Action<CustomSpecificationAttributeLocalizedModel, int> localizedModelConfiguration = null;

            model.IsRequired = true;

            if (customSpecificatonAttribute != null)
            {
                //fill in model values from the entity
                model ??= customSpecificatonAttribute.ToModel<CustomSpecificationAttributeModel>();

                //define localized model configuration action
                localizedModelConfiguration = async (locale, languageId) =>
                {
                    locale.DefaultValue = await _localizationService.GetLocalizedAsync(customSpecificatonAttribute, entity => entity.DefaultValue, languageId, false, false);
                };

                await PrepareConditionAttributesModelAsync(model.ConditionModel, customSpecificatonAttribute);
            }

            model.ConditionAllowed = true;

            if (!excludeProperties)
                model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);

            return model;
        }

        #endregion
    }
}
