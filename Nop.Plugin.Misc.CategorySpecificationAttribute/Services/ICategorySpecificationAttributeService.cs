using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;
using Nop.Plugin.Misc.CategorySpecificationAttribute.Models;

namespace Nop.Plugin.Misc.CategorySpecAttribute.Services
{
    public interface ICategorySpecificationAttributeService
    {
        Task CreateAsync(CategorySpecificationAttributeGroup model);

        Task CreateAsync(SpecificationAttributeGroupCategoryModel model);

        Task<IList<CategorySpecificationAttributeGroup>> GetBySpecificationAttributeGroupIdAsync(int specificationAttributeGroupId);

        Task<IList<CategorySpecificationAttributeGroup>> GetByCategoryIdAsync(int categoryId);
    }
}
