using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class ImageConversionOptions : ConversionOptions
    {
        public int? ImageWidth { get; set; }
        public int? ImageHeight { get; set; }
    }
}
