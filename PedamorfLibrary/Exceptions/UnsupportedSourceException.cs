using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class UnsupportedSourceException : ApplicationException
    {
        public string SourceName { get; set; }

        public UnsupportedSourceException(string message, string sourceName)
            : base(message)
        {
            this.SourceName = sourceName;
        }
    }
}
