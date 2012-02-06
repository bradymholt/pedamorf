using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class HtmlConversionOptions : ConversionOptions
    {
        public string HTTPAuthenticationUsername { get; set; }
        public string HTTPAuthenticationPassword { get; set; }
        public bool PrintMediaType { get; set; }
        public string UserStyleSheetUrl { get; set; }
    }
}
