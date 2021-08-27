using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Controllers
{
    //public partial class SpecificationAttributeController : Web.Areas.Admin.Controllers.SpecificationAttributeController
    //{
    //    #region Ctor

    //    public SpecificationAttributeController(ICustomerActivityService customerActivityService,
    //        ILocalizationService localizationService,
    //        ILocalizedEntityService localizedEntityService,
    //        INotificationService notificationService,
    //        IPermissionService permissionService,
    //        ISpecificationAttributeModelFactory specificationAttributeModelFactory,
    //        ISpecificationAttributeService specificationAttributeService) : base (
    //            customerActivityService,
    //            localizationService,
    //            localizedEntityService,
    //            notificationService,
    //            permissionService,
    //            specificationAttributeModelFactory,
    //            specificationAttributeService
    //            )
    //    {
    //    }

    //    #endregion

    //    public override Task<IActionResult> CreateSpecificationAttribute(SpecificationAttributeModel model, bool continueEditing)
    //    {
    //        Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
    //        return base.CreateSpecificationAttribute(model, continueEditing);
    //    }
    //}
}
