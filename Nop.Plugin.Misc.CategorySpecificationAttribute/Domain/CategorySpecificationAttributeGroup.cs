using Nop.Core;

namespace Nop.Plugin.Misc.CategorySpecAttribute.Domain
{
    public partial class CategorySpecificationAttributeGroup : BaseEntity
    {
        public int CategoryId { get; set; }

        public int SpecificationAttributeGroupId { get; set; }
    }
}
