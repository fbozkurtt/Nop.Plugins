using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping.Builders;
using Nop.Data.Extensions;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Mapping.Builders
{
    public class CustomSpecificationAttributeOptionBuilder : NopEntityBuilder<CustomSpecificationAttributeOption>
    {
        #region Methods

        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(CustomSpecificationAttributeOption.SpecificationAttributeOptionId)).AsInt32().Nullable().ForeignKey<SpecificationAttribute>();
        }

        #endregion
    }
}
