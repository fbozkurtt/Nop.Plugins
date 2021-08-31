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

        #region Ctor

        public CustomSpecificationAttributeService(IRepository<CustomSpecificationAttribute> customSpecificationAttributeRepository)
        {
            _customSpecificationAttributeRepository = customSpecificationAttributeRepository;
        }

        #endregion

        #region Methods

        public async Task InsertCustomSpecificationAttributeAsync(CustomSpecificationAttribute model) =>
            await _customSpecificationAttributeRepository.InsertAsync(model);

        public async Task<CustomSpecificationAttribute> GetBySpecificationAttributeIdAsync(int specificationAttributeId) =>
            (await _customSpecificationAttributeRepository.GetAllAsync(query =>
                query.Where(s => s.SpecificationAttributeId == specificationAttributeId))).FirstOrDefault();

        public async Task<CustomSpecificationAttribute> GetCustomSpecificationAttributeByIdAsync(int customSpecificationAttributeId) =>
            await _customSpecificationAttributeRepository.GetByIdAsync(customSpecificationAttributeId);

        public async Task<IList<CustomSpecificationAttribute>> GetAllCustomSpecificationAttributesAsync() =>
            await (await _customSpecificationAttributeRepository.GetAllAsync(query =>
                query)).ToListAsync();

        public async Task UpdateCustomSpecificationAttributeAsync(CustomSpecificationAttribute model) =>
            await _customSpecificationAttributeRepository.UpdateAsync(model);

        #endregion
    }
}
