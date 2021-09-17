using FluentMigrator;
using FluentMigrator.Infrastructure;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Migrations
{
    [NopMigration("2021/08/24 22:05:19:0000000", "Nop.Plugin.Misc.AdvancedSpecificationAttributes schema")]
    public class AdvancedSpecificationAttributesSchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<CategorySpecificationAttributeGroup>();
            Create.TableFor<CustomSpecificationAttribute>();
            Create.TableFor<CustomSpecificationAttributeOption>();
        }
    }
}
