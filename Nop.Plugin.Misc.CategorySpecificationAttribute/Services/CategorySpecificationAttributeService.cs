using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;

namespace Nop.Plugin.Misc.CategorySpecAttribute.Services
{
    class CategorySpecificationAttributeService : ICategorySpecificationAttributeService
    {
        private readonly IRepository<CategorySpecificationAttributeGroup> _categorySpecificationAttributeRepository;

        public CategorySpecificationAttributeService(IRepository<CategorySpecificationAttributeGroup> categorySpecificationAttributeRepository)
        {
            _categorySpecificationAttributeRepository = categorySpecificationAttributeRepository;
        }

        public virtual async Task CreateAsync(CategorySpecificationAttributeGroup model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            await _categorySpecificationAttributeRepository.InsertAsync(model);
        }

        public async Task<IList<CategorySpecificationAttributeGroup>> GetBySpecificationAttributeGroupId(int specificationAttributeGroupId) =>
            await _categorySpecificationAttributeRepository.GetAllAsync(query =>
            query.Where(c => c.SpecificationAttributeGroupId == specificationAttributeGroupId));

    }
}