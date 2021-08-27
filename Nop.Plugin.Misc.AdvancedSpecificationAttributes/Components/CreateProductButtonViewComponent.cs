using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Components
{
    [ViewComponent(Name = "CreateProductButton")]
    public class CreateProductButtonViewComponent : NopViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View("~/Plugins/Misc.AdvancedSpecificationAttributes/Views/Components/CreateProductButton.cshtml"));
        }
    }
}
