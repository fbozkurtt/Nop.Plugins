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

        Task<CustomSpecificationAttribute> GetBySpecificationAttributeIdAsync(int specificationAttributeId);

        #endregion
    }
}
