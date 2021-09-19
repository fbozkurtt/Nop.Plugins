using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Factories;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Controllers
{
    [AutoValidateAntiforgeryToken]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    public class CustomSpecificationAttributeController : BasePluginController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly INotificationService _notificationService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly ICustomSpecificationAttributeService _customSpecificationAttributeService;
        private readonly ICustomSpecificationAttributeParser _customSpecificationAttributeParser;
        private readonly ICategorySpecificationAttributeService _categorySpecificationAttributeService;
        private readonly ICustomSpecificationAttributeModelFactory _customSpecificationAttributeModelFactory;

        #endregion

        #region Ctor

        public CustomSpecificationAttributeController(ICustomerActivityService customerActivityService, IPermissionService permissionService, ILocalizationService localizationService, ILocalizedEntityService localizedEntityService, INotificationService notificationService, ISpecificationAttributeService specificationAttributeService, ICustomSpecificationAttributeService customSpecificationAttributeService, ICustomSpecificationAttributeParser customSpecificationAttributeParser, ICategorySpecificationAttributeService categorySpecificationAttributeService, ICustomSpecificationAttributeModelFactory customSpecificationAttributeModelFactory)
        {
            _customerActivityService = customerActivityService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _notificationService = notificationService;
            _specificationAttributeService = specificationAttributeService;
            _customSpecificationAttributeService = customSpecificationAttributeService;
            _customSpecificationAttributeParser = customSpecificationAttributeParser;
            _categorySpecificationAttributeService = categorySpecificationAttributeService;
            _customSpecificationAttributeModelFactory = customSpecificationAttributeModelFactory;
        }

        #endregion

        #region Utilities
        protected async Task UpdateAttributeLocalesAsync(CustomSpecificationAttribute customSpecificationAttribute, CustomSpecificationAttributeModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _localizedEntityService.SaveLocalizedValueAsync(customSpecificationAttribute,
                    x => x.DefaultValue,
                    localized.DefaultValue,
                    localized.LanguageId);
            }
        }

        protected async Task SaveConditionAttributesAsync(CustomSpecificationAttribute customSpecificationAttribute, CustomSpecificationAttributeModel model)
        {
            string attributesXml = null;

            if (model.ConditionModel.EnableCondition)
            {
                var attribute = await _customSpecificationAttributeService.GetCustomSpecificationAttributeByIdAsync(model.ConditionModel.SelectedAttributeId);
                if (attribute != null)
                {
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlType.DropdownList:
                        case AttributeControlType.RadioList:
                        case AttributeControlType.ColorSquares:
                        case AttributeControlType.ImageSquares:
                            {
                                var selectedAttribute = model.ConditionModel.ConditionAttributes
                                    .FirstOrDefault(x => x.Id == model.ConditionModel.SelectedAttributeId);
                                var selectedOptions = selectedAttribute?.SelectedOptionIds;

                                //for conditions we should empty values save even when nothing is selected
                                //otherwise "attributesXml" will be empty
                                //hence we won't be able to find a selected attribute
                                attributesXml = _customSpecificationAttributeParser.AddCustomSpecificationAttribute(null, attribute, selectedOptions ?? new List<string>());
                            }
                            break;
                        case AttributeControlType.Checkboxes:
                            {
                                var selectedAttribute = model.ConditionModel.ConditionAttributes
                                    .FirstOrDefault(x => x.Id == model.ConditionModel.SelectedAttributeId);
                                var selectedOptions = selectedAttribute?.Options
                                    .Where(x => x.Selected)
                                    .Select(x => x.Value)
                                    .ToList();

                                if (selectedOptions?.Any() ?? false)
                                        attributesXml = _customSpecificationAttributeParser.AddCustomSpecificationAttribute(attributesXml, attribute, selectedOptions);
                                else
                                    attributesXml = _customSpecificationAttributeParser.AddCustomSpecificationAttribute(null, attribute, new List<string>());
                            }
                            break;
                        case AttributeControlType.ReadonlyCheckboxes:
                        case AttributeControlType.TextBox:
                        case AttributeControlType.MultilineTextbox:
                        case AttributeControlType.Datepicker:
                        case AttributeControlType.FileUpload:
                        case AttributeControlType.Numeric:
                        case AttributeControlType.Decimal:
                        default:
                            //these attribute types are not supported as conditions
                            break;
                    }
                }
            }

            customSpecificationAttribute.ConditionAttributeXml = attributesXml;
        }

        #endregion

        #region Custom specification attributes

        [Area(AreaNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> EditCustomSpecificationAttribute(CustomSpecificationAttributeModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAttributes))
                return AccessDeniedView();

            //try to get a specification attribute with the specified id
            var specificationAttribute = await _specificationAttributeService.GetSpecificationAttributeByIdAsync(model.SpecificationAttributeId);
            if (specificationAttribute == null)
                return ErrorJson("Specification attribute not found");

            var customSpecificationAttribute = await _customSpecificationAttributeService.GetCustomSpecificationAttributeByIdAsync(model.Id);
            if (customSpecificationAttribute == null)
                return ErrorJson("Custom specification attribute not found");

            if (!ModelState.IsValid)
                return ErrorJson(ModelState.SerializeErrors());

            customSpecificationAttribute = model.ToEntity<CustomSpecificationAttribute>();
            await SaveConditionAttributesAsync(customSpecificationAttribute, model);
            await _customSpecificationAttributeService.UpdateCustomSpecificationAttributeAsync(customSpecificationAttribute);

            await UpdateAttributeLocalesAsync(customSpecificationAttribute, model);

            //_notificationService.ErrorNotification(await _localizationService.GetResourceAsync("Admin.Catalog.Attributes.CheckoutAttributes.Updated"));

            return Json(new { Result = true });
        }

        #endregion

        #region Custom specification attribute groups

        [Area(AreaNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddCategoriesToSpecificationAttributeGroup(SpecificationAttributeGroupCategoryModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAttributes))
                return Unauthorized();

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
                return Unauthorized();

            await _categorySpecificationAttributeService.CreateAsync(model);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecificationAttributesWithOptionsByCategory(int categoryId, bool includeNonGroupedAttributes = true)
        {
            var categorySpecAttributeGroups = await _categorySpecificationAttributeService.GetByCategoryIdAsync(categoryId);

            var result = (from c in categorySpecAttributeGroups
                          select new
                          {
                              name = _specificationAttributeService.GetSpecificationAttributeGroupByIdAsync(c.SpecificationAttributeGroupId).Result.Name,
                              specificationAttributes = (from s in _specificationAttributeService.GetSpecificationAttributesByGroupIdAsync(c.SpecificationAttributeGroupId).Result
                                                         select new
                                                         {
                                                             name = s.Name,
                                                             customSpecificationAttribute = _customSpecificationAttributeModelFactory.PrepareCustomSpecificationAttributeModelAsync(null, _customSpecificationAttributeService.GetBySpecificationAttributeIdAsync(s.Id).Result).Result,
                                                             options = (from o in _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttributeAsync(s.Id).Result
                                                                        select new
                                                                        {
                                                                            id = o.Id,
                                                                            name = o.Name
                                                                        }).ToList()
                                                         }).ToList()
                          }).ToList();

            if (includeNonGroupedAttributes)
            {
                result.Add(new
                {
                    name = string.Empty,
                    specificationAttributes = (from s in _specificationAttributeService.GetSpecificationAttributesByGroupIdAsync().Result
                                               select new
                                               {
                                                   name = s.Name,
                                                   customSpecificationAttribute = _customSpecificationAttributeModelFactory.PrepareCustomSpecificationAttributeModelAsync(null, _customSpecificationAttributeService.GetBySpecificationAttributeIdAsync(s.Id).Result).Result,
                                                   options = (from o in _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttributeAsync(s.Id).Result
                                                              select new
                                                              {
                                                                  id = o.Id,
                                                                  name = o.Name
                                                              }).ToList()
                                               }).ToList()
                });
            }

            return Json(result);
        }

        #endregion
    }
}
