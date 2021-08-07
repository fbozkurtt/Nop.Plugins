using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;
using Nop.Plugin.Misc.CategorySpecAttribute.Services;
using Nop.Plugin.Misc.CategorySpecificationAttribute.Factories;
using Nop.Plugin.Misc.CategorySpecificationAttribute.Models;
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
        private readonly ISpecificationAttributeGroupCategoryModelFactory _specificationAttributeGroupCategoryModelFactory;

        public CategorySpecificationAttributeGroupViewComponent(
            ICategoryService categoryService,
            ISpecificationAttributeService specificationAttributeService,
            ICategorySpecificationAttributeService categorySpecificationAttributeService,
            ISpecificationAttributeGroupCategoryModelFactory specificationAttributeGroupCategoryModelFactory)
        {
            _categoryService = categoryService;
            _specificationAttributeService = specificationAttributeService;
            _categorySpecificationAttributeService = categorySpecificationAttributeService;
            _specificationAttributeGroupCategoryModelFactory = specificationAttributeGroupCategoryModelFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            if (additionalData is not SpecificationAttributeGroupModel model)
                return Content(string.Empty);

            if (model.Id == 0)
                return Content(string.Empty);

            var avaliableCategories = await _categoryService.GetAllCategoriesAsync();

            var specificationAttributeGroupCategoryModel = await _specificationAttributeGroupCategoryModelFactory
                .PrepareSpecificationAttributeGroupCategoryModelAsync(new SpecificationAttributeGroupCategoryModel()
                {
                    Id = model.Id
                });

            if (avaliableCategories != null && avaliableCategories.Count > 0)
                return View("~/Plugins/Misc.CategorySpecificationAttribute/Views/_CreateOrUpdateSpecificationAttributeGroup.UsedByCategories.cshtml", specificationAttributeGroupCategoryModel);

            return Content(string.Empty);
        }
    }
}