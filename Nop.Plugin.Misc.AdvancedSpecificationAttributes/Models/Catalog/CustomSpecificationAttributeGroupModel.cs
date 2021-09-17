using Nop.Web.Areas.Admin.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models.Catalog
{
    public class CustomSpecificationAttributeGroupModel
    {
        public SpecificationAttributeGroupModel BaseModel { get; set; }

        public IDictionary<string, SpecificationAttributeModel> SpecificationAttributes { get; set; }
    }
}
