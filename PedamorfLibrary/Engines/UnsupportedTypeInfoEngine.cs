using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class UnsupportedTypeEngine : WebKitHtmlEngine
    {
        public UnsupportedTypeEngine(string fileName)
            : base(string.Format("<p><strong>{0}</strong> could not be converted because it is an unsupported file type.</p>", fileName)) { }

    }
}
