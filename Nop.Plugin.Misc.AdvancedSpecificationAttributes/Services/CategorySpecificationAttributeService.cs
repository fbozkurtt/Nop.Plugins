using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models;
using Nop.Services.Catalog;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Services
{
    public class CategorySpecificationAttributeService : ICategorySpecificationAttributeService
    {
        private readonly IRepository<CategorySpecificationAttributeGroup> _categorySpecificationAttributeRepository;
        private readonly ICategoryService _categoryService;
        private readonly ISpecificationAttributeService _specificationAttributeService;

        public CategorySpecificationAttributeService(IRepository<Domain.CategorySpecificationAttributeGroup> categorySpecificationAttributeRepository, ICategoryService categoryService, ISpecificationAttributeService specificationAttributeService)
        {
            _categorySpecificationAttributeRepository = categorySpecificationAttributeRepository;
            _categoryService = categoryService;
            _specificationAttributeService = specificationAttributeService;
        }

        public async Task CreateAsync(CategorySpecificationAttributeGroup model)
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

            foreach (var existingCategorySpecificationAttributeGroup in existingCategorySpecificationAttributeGroups)
                if (!model.SelectedCategoryIds.Contains(existingCategorySpecificationAttributeGroup.CategoryId))
                    await _categorySpecificationAttributeRepository.DeleteAsync(existingCategorySpecificationAttributeGroup);

            foreach (var categoryId in model.SelectedCategoryIds)
            {
                if (!existingCategorySpecificationAttributeGroups.Any(e => e.CategoryId == categoryId))
                {
                    await _categorySpecificationAttributeRepository.InsertAsync(new Domain.CategorySpecificationAttributeGroup() { CategoryId = categoryId, SpecificationAttributeGroupId = model.Id });
                }
            }
        }

        public async Task<IList<CategorySpecificationAttributeGroup>> GetBySpecificationAttributeGroupIdAsync(int specificationAttributeGroupId) =>
            await _categorySpecificationAttributeRepository.GetAllAsync(query =>
            query.Where(c => c.SpecificationAttributeGroupId == specificationAttributeGroupId));

        public async Task<IList<CategorySpecificationAttributeGroup>> GetByCategoryIdAsync(int categoryId) =>
            await _categorySpecificationAttributeRepository.GetAllAsync(query =>
            query.Where(c => c.CategoryId == categoryId));
    }
}