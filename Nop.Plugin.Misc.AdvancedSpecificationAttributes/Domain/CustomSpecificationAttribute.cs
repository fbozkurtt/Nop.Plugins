using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain
{
    public partial class CustomSpecificationAttribute : BaseEntity, ILocalizedEntity
    {
        public int SpecificationAttributeId { get; set; }

        public bool IsRequired { get; set; }

        public string ConditionAttributeXml { get; set; }

        public int? ValidationMinLength { get; set; }

        public int? ValidationMaxLength { get; set; }

        public string ValidationFileAllowedExtensions { get; set; }

        public int? ValidationFileMaximumSize { get; set; }

        public string DefaultValue { get; set; }

        public int AttributeFilterTypeId { get; set; }

        public int AttributeControlTypeId { get; set; }

        public AttributeFilterType AttributeFilterType
        {
            get => (AttributeFilterType)AttributeFilterTypeId;
            set => AttributeFilterTypeId = (int)value;
        }

        public AttributeControlType AttributeControlType
        {
            get => (AttributeControlType)AttributeControlTypeId;
            set => AttributeControlTypeId = (int)value;
        }
    }
}
