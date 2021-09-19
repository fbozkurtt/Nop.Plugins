using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Models.Catalog
{
    public partial record CustomProductModel : BaseNopEntityModel
    {
        public ProductModel BaseModel { get; set; }

        #region Nested Classes
        public partial record CheckoutAttributeModel : BaseNopEntityModel
        {
            public CheckoutAttributeModel()
            {
                AllowedFileExtensions = new List<string>();
                Options = new List<CheckoutAttributeValueModel>();
            }

            public string Name { get; set; }

            public string DefaultValue { get; set; }

            public string TextPrompt { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Selected day value for datepicker
            /// </summary>
            public int? SelectedDay { get; set; }
            /// <summary>
            /// Selected month value for datepicker
            /// </summary>
            public int? SelectedMonth { get; set; }
            /// <summary>
            /// Selected year value for datepicker
            /// </summary>
            public int? SelectedYear { get; set; }

            /// <summary>
            /// Allowed file extensions for customer uploaded files
            /// </summary>
            public IList<string> AllowedFileExtensions { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public AttributeFilterType AttributeFilterType { get; set; }

            public IList<CheckoutAttributeValueModel> Options { get; set; }
        }

        public partial record CheckoutAttributeValueModel : BaseNopEntityModel
        {
            public string Name { get; set; }

            public string ColorSquaresRgb { get; set; }

            public string PriceAdjustment { get; set; }

            public bool IsPreSelected { get; set; }
        }

        #endregion
    }
}
