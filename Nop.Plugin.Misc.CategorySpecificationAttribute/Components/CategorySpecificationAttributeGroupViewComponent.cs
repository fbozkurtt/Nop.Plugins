using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;
using Nop.Plugin.Misc.CategorySpecAttribute.Services;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.CategorySpecAttribute.Components
{
    [ViewComponent(Name = "CategorySpecificationAttributeGroup")]
    public class CategorySpecificationAttributeGroupViewComponent : NopViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly ICategorySpecificationAttributeService _categorySpecificationAttributeService;

        public CategorySpecificationAttributeGroupViewComponent(
            ICategoryService categoryService,
            ISpecificationAttributeService specificationAttributeService,
            ICategorySpecificationAttributeService categorySpecificationAttributeService)
        {
            _categoryService = categoryService;
            _specificationAttributeService = specificationAttributeService;
            _categorySpecificationAttributeService = categorySpecificationAttributeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            if (additionalData is not SpecificationAttributeGroupModel model)
                return Content(string.Empty);

            if (model.Id == 0)
                return Content(string.Empty);

            var categories = await _categoryService.GetAllCategoriesAsync();

            var categorySpecAttributeGroups = await _categorySpecificationAttributeService.GetBySpecificationAttributeGroupId(model.Id) as List<CategorySpecificationAttributeGroup>;

            if (categories != null && categories.Count > 0)
                return View("~/Plugins/Misc.CategorySpecAttribute/Views/_CreateOrUpdateSpecificationAttributeGroup.UsedByCategories.cshtml", (categories, model, categorySpecAttributeGroups));

            return Content(string.Empty);
        }
    }
}