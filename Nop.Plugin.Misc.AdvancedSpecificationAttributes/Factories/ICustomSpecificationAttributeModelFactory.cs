using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Factories
{
    public interface ICustomSpecificationAttributeModelFactory
    {
        Task<CustomSpecificationAttributeModel> PrepareCustomSpecificationAttributeModelAsync(CustomSpecificationAttributeModel model,
            CustomSpecificationAttribute customSpecificatonAttribute, bool excludeProperties = false);
    }
}
