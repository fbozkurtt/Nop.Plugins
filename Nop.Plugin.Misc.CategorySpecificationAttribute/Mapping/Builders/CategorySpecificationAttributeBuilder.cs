using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping.Builders;
using Nop.Data.Extensions;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;

namespace Nop.Plugin.Misc.CategorySpecAttribute.Mapping.Builders
{
    public class CategorySpecificationAttributGroupBuilder : NopEntityBuilder<Domain.CategorySpecificationAttributeGroup>
    {
        #region Methods

        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(Domain.CategorySpecificationAttributeGroup.CategoryId)).AsInt32().Nullable().ForeignKey<Category>();
            table.WithColumn(nameof(Domain.CategorySpecificationAttributeGroup.SpecificationAttributeGroupId)).AsInt32().Nullable().ForeignKey<SpecificationAttributeGroup>();
        }

        #endregion
    }
}