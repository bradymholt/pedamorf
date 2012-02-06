using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class ConversionEngineException : ApplicationException
    {
        public string ConversionType { get; set; }
        public string EngineName { get; set; }
        public string EngineProcessOutput { get; set; }

        public ConversionEngineException(string message, string engineProcessOutput, string conversionType, ConversionEngine engine)
            : this(message, engineProcessOutput, conversionType, engine, null)
        {
        }

        public ConversionEngineException(string message, string engineProcessOutput, string conversionType, ConversionEngine engine, Exception innerException)
            : base(message, innerException)
        {
            this.ConversionType = conversionType;
            this.EngineName = engine.GetType().Name;
            this.EngineProcessOutput = engineProcessOutput;
        }
    }
}
