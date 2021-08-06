using FluentMigrator;
using FluentMigrator.Infrastructure;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.CategorySpecAttribute.Domain;

namespace Nop.Plugin.Misc.CategorySpecAttribute.Migrations
{
    [SkipMigrationOnUpdate]
    [NopMigration("2021/08/05 22:05:19:0000000", "Nop.Plugin.Misc.CategorySpecAttribute schema")]
    public class SchemaMigration : AutoReversingMigration
    {
        private readonly IMigrationManager _migrationManager;

        public SchemaMigration(IMigrationManager migrationManager)
        {
            _migrationManager = migrationManager;
        }

        /// <summary>
        /// Collect the UP migration expressions
        /// </summary>
        public override void Up()
        {
            _migrationManager.BuildTable<CategorySpecificationAttributeGroup>(Create);
        }
    }
}
