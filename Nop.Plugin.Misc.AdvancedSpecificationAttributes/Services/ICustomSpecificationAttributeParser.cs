using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services
{
    public interface ICustomSpecificationAttributeParser
    {
        Task<IList<CustomSpecificationAttribute>> ParseCustomSpecificationAttributesAsync(string attributesXml);

        IAsyncEnumerable<(CustomSpecificationAttribute attribute, IAsyncEnumerable<SpecificationAttributeOption> options)> ParseSpecificationAttributeOptions(string attributesXml);

        IList<string> ParseOptions(string attributesXml, int customSpecificationAttributeId);

        string AddCustomSpecificationAttribute(string attributesXml, CustomSpecificationAttribute customSpecificationAttribute, IList<string> options);

        Task<bool?> IsConditionMetAsync(CustomSpecificationAttribute customSpecificationAttribute, string selectedAttributesXml);

        string RemoveCustomSpecificationAttribute(string attributesXml, CustomSpecificationAttribute customSpecificationAttribute);
    }
}
