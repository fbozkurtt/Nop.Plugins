using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;

namespace Nop.Plugin.Misc.CategorySpecAttribute.Services
{
    public interface ICategorySpecificationAttributeService
    {
        Task CreateAsync(CategorySpecificationAttributeGroup model);

        Task<IList<CategorySpecificationAttributeGroup>> GetBySpecificationAttributeGroupId(int specificationAttributeGroupId);
    }
}
