using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;
using Nop.Plugin.Misc.CategorySpecAttribute.Services;
using Nop.Plugin.Misc.CategorySpecificationAttribute.Models;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.CategorySpecificationAttribute.Controllers
{
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    public class CategorySpecificationAttributeController : BasePluginController
    {
        private readonly IPermissionService _permissionService;
        private readonly ICategorySpecificationAttributeService _categorySpecificationAttributeService;

        public CategorySpecificationAttributeController(IPermissionService permissionService, ICategorySpecificationAttributeService categorySpecificationAttributeService)
        {
            _permissionService = permissionService;
            _categorySpecificationAttributeService = categorySpecificationAttributeService;
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(SpecificationAttributeGroupCategoryModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAttributes))
                return Unauthorized();

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
                return Unauthorized();

            await _categorySpecificationAttributeService.CreateAsync(model);

            return Ok();
        }
    }
}
