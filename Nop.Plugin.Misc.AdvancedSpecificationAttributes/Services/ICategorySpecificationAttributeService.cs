using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services
{
    public interface ICategorySpecificationAttributeService
    {
        Task CreateAsync(CategorySpecificationAttributeGroup model);

        Task CreateAsync(SpecificationAttributeGroupCategoryModel model);

        Task<IList<CategorySpecificationAttributeGroup>> GetBySpecificationAttributeGroupIdAsync(int specificationAttributeGroupId);

        Task<IList<CategorySpecificationAttributeGroup>> GetByCategoryIdAsync(int categoryId);
    }
}
