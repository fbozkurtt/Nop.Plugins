using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.CategorySpecificationAttribute.Models
{
    public class CategorySpecificationAttributeGroupModel
    {
        public int Id { get; set; }
        public IList<int> SelectedSpecificationAttributeGroupIds { get; set; }
        public IList<SelectListItem> AvailableSpecificationGroups { get; set; }
    }
}
