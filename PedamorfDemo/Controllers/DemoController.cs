using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Pedamorf.Service.Client;
using Pedamorf.Demo.Models;

namespace PedamorfDemo.Controllers
{
    public class DemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Convert(DemoModel model)
        {
            PedamorfResponse response = null;
            try
            {
                using (PedamorfServiceClient client = PedamorfServiceManager.GetClient(model.ServiceHost))
                {
                    if (!string.IsNullOrEmpty(model.UrlButton))
                    {
                        response = client.ConvertUrl(model.Url, new HtmlConversionOptions() { Orientation = model.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait });
                    }
                    else if (!string.IsNullOrEmpty(model.HtmlButton))
                    {
                        response = client.ConvertHtml(model.Html, new HtmlConversionOptions() { Orientation = model.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait });
                    }
                    else if (!string.IsNullOrEmpty(model.FileButton) && model.File1.ContentLength > 0)
                    {
                        response = client.ConvertFile(model.File1.InputStream, model.File1.FileName, model.File2.InputStream, model.File2.FileName, new ConversionOptions() { Orientation = model.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait });

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
            }

            if (!response.Error)
            {
                return File(response.ResultPdf, "application/pdf");
            }
            else
            {
                TempData["error"] = response.ErrorMessage;
                return View();
            }
        }
    }
}
