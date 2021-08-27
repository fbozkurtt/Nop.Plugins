using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.AdvancedSpecificationAttributes.Domain
{
    public enum AttributeControlType
    {
        DropdownList = 1,
        RadioList = 2,
        Checkboxes = 3,
        TextBox = 4,
        MultilineTextbox = 10,
        Datepicker = 20,
        FileUpload = 30,
        ColorSquares = 40,
        ImageSquares = 45,
        ReadonlyCheckboxes = 50,
        Numeric = 55,
        Decimal = 60
    }
}
