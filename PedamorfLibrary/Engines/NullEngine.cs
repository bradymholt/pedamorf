using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class NullEngine : ConversionEngine
    {
        public NullEngine() { }

        protected override void Convert(ConversionOptions options)
        {
            this.OutputFile = null;
        }
    }
}
