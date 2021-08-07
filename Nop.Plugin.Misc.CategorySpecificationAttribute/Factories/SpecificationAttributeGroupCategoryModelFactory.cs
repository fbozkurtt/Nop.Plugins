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

        public SpecificationAttributeGroupCategoryModelFactory(IBaseAdminModelFactory baseAdminModelFactory)
        {
            _baseAdminModelFactory = baseAdminModelFactory;
        }

        public async Task<SpecificationAttributeGroupCategoryModel> PrepareSpecificationAttributeGroupCategoryModelAsync(SpecificationAttributeGroupCategoryModel model)
        {
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
