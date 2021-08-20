using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Controllers
{
    public class CustomSpecificationAttributeController: BaseAdminController
    {
        #region Fields

        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        #endregion

        //public async Task<IActionResult> Create()
        //{
        //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAttributes))
        //        return AccessDeniedView();

        //    //prepare model
        //    var model = await _checkoutAttributeModelFactory.PrepareCheckoutAttributeModelAsync(new CheckoutAttributeModel(), null);

        //    return View(model);
        //}
    }
}
