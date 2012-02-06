using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class UrlFetchException : ConversionEngineException
    {
        private string[] Urls { get; set; }

        public UrlFetchException(string[] url, string engineProcessOutput, ConversionEngine engine)
            : base("Fetch Url(s) failed.", engineProcessOutput, "Url", engine)
        {
        }
    }
}
