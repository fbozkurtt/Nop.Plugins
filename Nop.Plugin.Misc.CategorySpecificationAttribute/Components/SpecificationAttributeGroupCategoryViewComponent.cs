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
        #region Fields

        private readonly ICategorySpecificationAttributeGroupModelFactory _specificationAttributeGroupCategoryModelFactory;

        #endregion

        #region Ctor

        public CategorySpecificationAttributeGroupViewComponent(ICategorySpecificationAttributeGroupModelFactory specificationAttributeGroupCategoryModelFactory)
        {
            _specificationAttributeGroupCategoryModelFactory = specificationAttributeGroupCategoryModelFactory;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            if (additionalData is not SpecificationAttributeGroupModel model)
                return Content(string.Empty);

            if (model.Id == 0)
                return Content(string.Empty);

            var specificationAttributeGroupCategoryModel = await _specificationAttributeGroupCategoryModelFactory
                .PrepareSpecificationAttributeGroupCategoryModelAsync(new SpecificationAttributeGroupCategoryModel()
                {
                    Id = model.Id
                });

            return View("~/Plugins/Misc.CategorySpecificationAttributeGroup/Views/SpecificationAttribute/_CreateOrUpdateSpecificationAttributeGroup.Categories.cshtml", specificationAttributeGroupCategoryModel);
        }

        #endregion
    }
}