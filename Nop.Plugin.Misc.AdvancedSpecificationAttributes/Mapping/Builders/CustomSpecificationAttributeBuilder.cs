using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping.Builders;
using Nop.Data.Extensions;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Mapping.Builders
{
    public class CustomSpecificationAttributeBuilder : NopEntityBuilder<CustomSpecificationAttribute>
    {
        #region Methods

        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(CustomSpecificationAttribute.SpecificationAttributeId)).AsInt32().Nullable().ForeignKey<SpecificationAttribute>();
        }

        #endregion
    }
}