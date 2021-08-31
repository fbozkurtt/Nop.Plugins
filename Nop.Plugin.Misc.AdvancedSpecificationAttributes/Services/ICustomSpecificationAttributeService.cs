using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Services.Catalog;
using Nop.Services.Security;
using Nop.Services.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services
{
    public interface ICustomSpecificationAttributeService
    {
        #region Fields

        #endregion

        #region Ctor

        #endregion

        #region Methods

        Task InsertCustomSpecificationAttributeAsync(CustomSpecificationAttribute model);

        Task UpdateCustomSpecificationAttributeAsync(CustomSpecificationAttribute model);

        Task<CustomSpecificationAttribute> GetBySpecificationAttributeIdAsync(int specificationAttributeId);

        Task<CustomSpecificationAttribute> GetCustomSpecificationAttributeByIdAsync(int customSpecificationAttributeId);

        Task<IList<CustomSpecificationAttribute>> GetAllCustomSpecificationAttributesAsync();

        #region Custom specification attribute options

        //Task<CustomSpecificationAttributeOption> GetCustomSpecificationAttributeOptionByIdAsync(int customSpecificationAttributeOptionId);

        //Task<IList<CustomSpecificationAttributeOption>> GetCustomSpecificationAttributeOptionsBySpecificationAttributeIdAsync(int customSpecificationAttributeId);

        #endregion

        #endregion
    }
}
