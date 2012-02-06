using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pedamorf.Library
{
    public class WebKitRemoteEngine : WebKitEngine
    {
        protected string[] m_sourceUrls;
        public WebKitRemoteEngine(string url)
        {
            m_sourceUrls = new string[] { url };
        }

        public WebKitRemoteEngine(string[] urls)
        {
            m_sourceUrls = urls;
        }

        protected override void Convert(ConversionOptions options)
        {
            ConvertToPdf(m_sourceUrls, options);
        }
    }
}
