using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class MissingSourceException : ApplicationException
    {
        public MissingSourceException(string message)
            : base(message) { }
    }
}
