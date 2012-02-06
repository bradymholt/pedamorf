using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pedamorf.Library
{
    public class WebKitHtmlEngine : WebKitFileEngine
    {
        public WebKitHtmlEngine(string html) : this(new string[] { html }) { }
        public WebKitHtmlEngine(string[] htmls)
        {
            m_sourceFiles = new Dictionary<string, byte[]>();
            foreach (string html in htmls)
            {
                string htmlToEncode = html;
                if (!html.Contains("<html"))
                {
                    //source is fragment so wrap in full html body
                    htmlToEncode = string.Format("<html><body>{0}</body></html>", html);
                }

                UTF8Encoding enc = new UTF8Encoding();
                byte[] encodedHtml = enc.GetBytes(htmlToEncode);
                m_sourceFiles.Add(Guid.NewGuid().ToString() + ".html", encodedHtml);
            }
        }

       
    }
}
