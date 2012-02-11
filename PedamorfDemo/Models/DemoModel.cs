using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pedamorf.Demo.Models
{
    public class DemoModel
    {
        public string ServiceHost { get; set; }
        public bool Landscape { get; set; }

        public string Url { get; set; }

        [AllowHtml]
        public string Html { get; set; }

        public HttpPostedFileBase Document1 { get; set; }
        public HttpPostedFileBase Document2 { get; set; }

        public HttpPostedFileBase Image { get; set; }
     
        public string ImageWidth { get; set; }
        public string ImageHeight { get; set; }

        public string UrlButton { get; set; }
        public string HtmlButton { get; set; }
        public string DocumentButton { get; set; }
        public string ImageButton { get; set; }
    }
}
