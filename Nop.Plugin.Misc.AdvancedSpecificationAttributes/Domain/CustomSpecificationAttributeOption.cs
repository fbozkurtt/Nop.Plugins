using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain
{
    public class CustomSpecificationAttributeOption : BaseEntity
    {
        public int SpecificationAttributeOptionId { get; set; }

        public bool IsPreSelected { get; set; }
    }
}
