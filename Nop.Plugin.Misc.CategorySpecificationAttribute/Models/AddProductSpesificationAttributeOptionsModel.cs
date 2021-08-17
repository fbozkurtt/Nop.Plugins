using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.CategorySpecificationAttribute.Models
{
    public class AddProductSpesificationAttributeOptionsModel
    {
        public int ProductId { get; set; }
        public int[] SpecificationAttributeOptionIds { get; set; }
    }
}
