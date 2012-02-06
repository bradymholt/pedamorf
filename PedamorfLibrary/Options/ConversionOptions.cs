using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class ConversionOptions
    {
        public PageOrientation Orientation { get; set; }
        public bool SilentFailOnUnsupportedType { get; set; }
        public bool UsePlaceholderPageOnUnsupportedType { get; set; }
        public int TimeoutInSeconds { get; set; }
    }
}
