using FluentMigrator;
using FluentMigrator.Infrastructure;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;

namespace Nop.Plugin.Misc.CategorySpecAttribute.Migrations
{
    [SkipMigrationOnUpdate]
    [NopMigration("2021/08/05 22:05:19:0000000", "Nop.Plugin.Misc.CategorySpecAttribute schema")]
    public class AdvancedSpecificationAttributesSchemaMigration : AutoReversingMigration
    {
        private readonly IMigrationManager _migrationManager;

        public AdvancedSpecificationAttributesSchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            _migrationManager.BuildTable<CategorySpecificationAttributeGroup>(Create);
            _migrationManager.BuildTable<CustomSpecificationAttribute>(Create);
            _migrationManager.BuildTable<CustomSpecificationAttributeOption>(Create);
        }
    }
}
