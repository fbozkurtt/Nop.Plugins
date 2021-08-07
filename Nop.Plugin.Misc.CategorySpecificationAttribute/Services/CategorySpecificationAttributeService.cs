using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;
using Nop.Plugin.Misc.CategorySpecificationAttribute.Models;

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

        public async Task CreateAsync(SpecificationAttributeGroupCategoryModel model)
        {
            var existingCategorySpecificationAttributeGroups = await _categorySpecificationAttributeRepository.GetAllAsync(query =>
            {
                return query.Where(c => c.SpecificationAttributeGroupId == model.Id);
            });
            (model.SelectedCategoryIds as List<int>).ForEach((id) =>
            {
                if (!existingCategorySpecificationAttributeGroups.Any(e => e.CategoryId == id))
                    _categorySpecificationAttributeRepository.InsertAsync(new CategorySpecificationAttributeGroup() { CategoryId = id, SpecificationAttributeGroupId = model.Id });
            });
        }

        public async Task<IList<CategorySpecificationAttributeGroup>> GetBySpecificationAttributeGroupId(int specificationAttributeGroupId) =>
            await _categorySpecificationAttributeRepository.GetAllAsync(query =>
            query.Where(c => c.SpecificationAttributeGroupId == specificationAttributeGroupId));

    }
}