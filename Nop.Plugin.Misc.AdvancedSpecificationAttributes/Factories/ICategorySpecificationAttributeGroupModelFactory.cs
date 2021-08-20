using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Factories
{
    public interface ICategorySpecificationAttributeGroupModelFactory
    {
        public Task<SpecificationAttributeGroupCategoryModel> PrepareSpecificationAttributeGroupCategoryModelAsync(SpecificationAttributeGroupCategoryModel model);
    }
}
