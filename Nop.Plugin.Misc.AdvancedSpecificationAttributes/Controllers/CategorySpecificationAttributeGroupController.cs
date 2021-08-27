using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
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
    public class CategorySpecificationAttributeGroupController : BasePluginController
    {
        private readonly IPermissionService _permissionService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly ICategorySpecificationAttributeService _categorySpecificationAttributeService;

        public CategorySpecificationAttributeGroupController(IPermissionService permissionService, ISpecificationAttributeService specificationAttributeService, ICategorySpecificationAttributeService categorySpecificationAttributeService)
        {
            _permissionService = permissionService;
            _specificationAttributeService = specificationAttributeService;
            _categorySpecificationAttributeService = categorySpecificationAttributeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpecificationAttributeGroupCategoryModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageAttributes))
                return Unauthorized();

            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
                return Unauthorized();

            await _categorySpecificationAttributeService.CreateAsync(model);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecificationAttributesByCategoryId(int categoryId, bool includeNonGroupedAttributes = true)
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
                    name = "default",
                    specificationAttributes = (from s in _specificationAttributeService.GetSpecificationAttributesByGroupIdAsync().Result
                                               select new
                                               {
                                                   name = s.Name,
                                                   options = (from o in _specificationAttributeService.GetSpecificationAttributeOptionsBySpecificationAttributeAsync(s.Id).Result
                                                              select new
                                                              {
                                                                  id = o.Id,
                                                                  name = o.Name
                                                              }).ToList()
                                               }).ToList()
                });
            }

            return Ok(result);
        }

    }
}
