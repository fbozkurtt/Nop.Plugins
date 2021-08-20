using Nop.Data;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services
{
    public class CustomSpecificationAttributeService : ICustomSpecificationAttributeService
    {
        #region Fields 

        private readonly IRepository<CustomSpecificationAttribute> _customSpecificationAttributeRepository;

        #endregion

        #region Methods

        public async Task<CustomSpecificationAttribute> GetBySpecificationAttributeIdAsync(int specificationAttributeId) =>
            (await _customSpecificationAttributeRepository.GetAllAsync(query => 
                query.Where(s => s.SpecificationAttributeId == specificationAttributeId))).FirstOrDefault();

        #endregion
    }
}
