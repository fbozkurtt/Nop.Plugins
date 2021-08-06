using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.CategorySpecAttribute
{
    /// <summary>
    /// Rename this file and change to the correct type
    /// </summary>
    public class CategorySpecAttributePlugin : BasePlugin, IWidgetPlugin
    {
        public bool HideInWidgetList => false;

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "CategorySpecificationAttributeGroup";
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { AdminWidgetZones.SpecificationAttributeGroupDetailsBlock });
        }
    }
}
