using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes
{
    /// <summary>
    /// Represents plugin default vaues and constants
    /// </summary>
    public class AdvancedSpecificationAttributesDefaults
    {
        /// <summary>
        /// Gets the plugin system name
        /// </summary>
        public static string SystemName => "Widgets.AccessiBe";

        /// <summary>
        /// Gets the user agent used to request third-party services
        /// </summary>
        public static string UserAgent => $"nopcommerce-{NopVersion.CURRENT_VERSION}";

        /// <summary>
        /// Gets the configuration route name
        /// </summary>
        public static string ConfigurationRouteName => "Plugin.Misc.AdvancedSpecificationAttributes.Configure";

        /// <summary>
        /// Gets a name of the view component to display payment info in public store
        /// </summary>
        public const string CUSTOM_SPECIFICATION_ATTRIBUTE_VIEW_COMPONENT_NAME = "CustomSpecificationAttribute";

        /// <summary>
        /// Gets a name of the view component to add script to pages
        /// </summary>
        public const string SCRIPT_VIEW_COMPONENT_NAME = "PayPalSmartPaymentButtonsScript";

        /// <summary>
        /// Gets a name of the view component to display buttons
        /// </summary>
        public const string BUTTONS_VIEW_COMPONENT_NAME = "PayPalSmartPaymentButtonsButtons";

        /// <summary>
        /// Gets a name of the view component to display logo
        /// </summary>
        public const string LOGO_VIEW_COMPONENT_NAME = "PayPalSmartPaymentButtonsLogo";
    }
}
