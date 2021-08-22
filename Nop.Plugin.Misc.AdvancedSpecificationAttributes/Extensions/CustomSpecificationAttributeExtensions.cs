using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Extensions
{
    public static class CustomSpecificationAttributeExtensions
    { 
        public static bool ShouldHaveValues(this CustomSpecificationAttribute customSpecificationAttribute)
        {
            if (customSpecificationAttribute == null)
                return false;

            if (customSpecificationAttribute.AttributeControlType == AttributeControlType.TextBox ||
                customSpecificationAttribute.AttributeControlType == AttributeControlType.MultilineTextbox ||
                customSpecificationAttribute.AttributeControlType == AttributeControlType.Datepicker ||
                customSpecificationAttribute.AttributeControlType == AttributeControlType.FileUpload)
                return false;

            //other attribute control types support values
            return true;
        }

        public static bool CanBeUsedAsCondition(this CustomSpecificationAttribute customSpecificationAttribute)
        {
            if (customSpecificationAttribute == null)
                return false;

            if (customSpecificationAttribute.AttributeControlType == AttributeControlType.ReadonlyCheckboxes ||
                customSpecificationAttribute.AttributeControlType == AttributeControlType.TextBox ||
                customSpecificationAttribute.AttributeControlType == AttributeControlType.MultilineTextbox ||
                customSpecificationAttribute.AttributeControlType == AttributeControlType.Datepicker ||
                customSpecificationAttribute.AttributeControlType == AttributeControlType.FileUpload)
                return false;

            //other attribute control types support it
            return true;
        }
    }
}