using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
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
    public class AdvancedSpecificationAttributeController : BasePluginController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly INotificationService _notificationService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly ICustomSpecificationAttributeService _customSpecificationAttributeService;

        #endregion

        #region Ctor

        public AdvancedSpecificationAttributeController(ICustomerActivityService customerActivityService, IPermissionService permissionService, ILocalizationService localizationService, ILocalizedEntityService localizedEntityService, INotificationService notificationService, ISpecificationAttributeService specificationAttributeService, ICustomSpecificationAttributeService customSpecificationAttributeService)
        {
            _customerActivityService = customerActivityService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _localizedEntityService = localizedEntityService;
            _notificationService = notificationService;
            _specificationAttributeService = specificationAttributeService;
            _customSpecificationAttributeService = customSpecificationAttributeService;
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

        #endregion

        #region Methods

        [HttpPost]
        public async Task<IActionResult> EditCustomSpecificationAttribute(CustomSpecificationAttributeModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAttributes))
                return AccessDeniedView();

            //try to get a specification attribute with the specified id
            var specificationAttribute = await _specificationAttributeService.GetSpecificationAttributeByIdAsync(model.SpecificationAttributeId);
            if (specificationAttribute == null)
                return RedirectToAction("List", "SpecificationAttribute");

            if (!ModelState.IsValid)
                return ErrorJson(ModelState.SerializeErrors());

            var customSpecificationAttribute = model.ToEntity<CustomSpecificationAttribute>();
            await _customSpecificationAttributeService.UpdateCustomSpecificationAttributeAsync(customSpecificationAttribute);

            await UpdateAttributeLocalesAsync(customSpecificationAttribute, model);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return Json(new { Result = true });
        }

        #endregion

    }
}
