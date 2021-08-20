using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models.Catalog;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Factories
{
    public class CustomSpecificationAttributeModelFactory : ICustomSpecificationAttributeModelFactory
    {
        public async Task<CustomSpecificationAttributeModel> PrepareCustomSpecificationAttributeModelAsync(CustomSpecificationAttributeModel model, CustomSpecificationAttribute customSpecificatonAttribute, bool excludeProperties = false)
        {
            model.IsRequired = false;

            if (customSpecificatonAttribute != null)
            {
                //fill in model values from the entity
                model ??= customSpecificatonAttribute.ToModel<CustomSpecificationAttributeModel>();
            }

            return model;
        }
    }
}
