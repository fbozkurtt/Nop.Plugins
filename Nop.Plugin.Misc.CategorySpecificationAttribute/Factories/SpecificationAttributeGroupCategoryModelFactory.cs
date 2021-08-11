using Nop.Plugin.Misc.CategorySpecAttribute.Services;
using Nop.Plugin.Misc.CategorySpecificationAttribute.Models;
using Nop.Web.Areas.Admin.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.CategorySpecificationAttribute.Factories
{
    public class SpecificationAttributeGroupCategoryModelFactory : ISpecificationAttributeGroupCategoryModelFactory
    {
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ICategorySpecificationAttributeService _categorySpecificationAttributeService;

        public SpecificationAttributeGroupCategoryModelFactory(IBaseAdminModelFactory baseAdminModelFactory, ICategorySpecificationAttributeService categorySpecificationAttributeService)
        {
            _baseAdminModelFactory = baseAdminModelFactory;
            _categorySpecificationAttributeService = categorySpecificationAttributeService;
        }

        public async Task<SpecificationAttributeGroupCategoryModel> PrepareSpecificationAttributeGroupCategoryModelAsync(SpecificationAttributeGroupCategoryModel model)
        {
            model.SelectedCategoryIds = (await _categorySpecificationAttributeService.GetBySpecificationAttributeGroupId(model.Id))
                        .Select(c => c.CategoryId).ToList();

            await _baseAdminModelFactory.PrepareCategoriesAsync(model.AvailableCategories, false);
            foreach (var categoryItem in model.AvailableCategories)
            {
                categoryItem.Selected = int.TryParse(categoryItem.Value, out var categoryId)
                    && model.SelectedCategoryIds.Contains(categoryId);
            }

            return model;
        }
    }
}
