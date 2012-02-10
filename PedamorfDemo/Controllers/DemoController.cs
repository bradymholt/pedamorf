using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Pedamorf.Service.Client;

namespace PedamorfDemo.Controllers
{
    public class DemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Convert(string txtServiceHost, string txtUrl, HttpPostedFileBase upFile, string txtHtml, string btnUrl, string btnHtml, string btnFile)
        {
            PedamorfResponse response = null;
            using (PedamorfServiceClient client = PedamorfServiceManager.GetClient(txtServiceHost))
            {
                if (!string.IsNullOrEmpty(btnUrl))
                {
                    response = client.ConvertUrl(txtHtml, new HtmlConversionOptions() { UsePlaceholderPageOnUnsupportedType = true });
                }
                else if (!string.IsNullOrEmpty(btnHtml))
                {
                    response = client.ConvertHtml(txtHtml, new HtmlConversionOptions() { UsePlaceholderPageOnUnsupportedType = true });
                }
                else if (!string.IsNullOrEmpty(btnHtml) && upFile.ContentLength > 0)
                {
                    response = client.ConvertFile(upFile.InputStream, upFile.FileName, new ConversionOptions() { UsePlaceholderPageOnUnsupportedType = true });
                }
            }

            return File(response.ResultPdf, "application/pdf");
        }
    }
}
