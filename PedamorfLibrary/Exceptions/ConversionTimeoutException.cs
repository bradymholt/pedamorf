using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class ConversionTimeoutException : ConversionEngineException
    {
        public ConversionTimeoutException(string message, string engineProcessOutput, string conversionType, ConversionEngine engine)
            : base(message, engineProcessOutput, conversionType, engine, null)
        {
        }
    }
}
