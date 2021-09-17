using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models;
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

        #region Custom specification attribute group

        Task InsertCategorySpecificationAttributeGroupAsync(SpecificationAttributeGroupCategoryModel specificationAttributeGroupCategoryModel);

        Task<IList<CategorySpecificationAttributeGroup>> GetBySpecificationAttributeGroupIdAsync(int specificationAttributeGroupId);

        Task<IList<CategorySpecificationAttributeGroup>> GetByCategoryIdAsync(int categoryId);

        #endregion

        #region Custom specification attribute

        Task InsertCustomSpecificationAttributeAsync(CustomSpecificationAttribute customSpecificationAttribute);

        Task UpdateCustomSpecificationAttributeAsync(CustomSpecificationAttribute customSpecificationAttribute);

        Task<CustomSpecificationAttribute> GetBySpecificationAttributeIdAsync(int specificationAttributeId);

        Task<CustomSpecificationAttribute> GetCustomSpecificationAttributeByIdAsync(int customSpecificationAttributeId);

        Task<IList<CustomSpecificationAttribute>> GetAllCustomSpecificationAttributesAsync();

        #endregion

        #region

        #region Custom specification attribute option

        //Task<CustomSpecificationAttributeOption> GetCustomSpecificationAttributeOptionByIdAsync(int customSpecificationAttributeOptionId);

        //Task<IList<CustomSpecificationAttributeOption>> GetCustomSpecificationAttributeOptionsBySpecificationAttributeIdAsync(int customSpecificationAttributeId);

        #endregion

        #endregion
    }
}
