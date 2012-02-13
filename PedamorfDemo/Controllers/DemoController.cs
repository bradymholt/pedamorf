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
                        response = client.ConvertUrl(model.Url,
                            new HtmlConversionOptions() { Orientation = model.Landscape ? PageOrientation.Landscape : PageOrientation.Portrait });
                    }
                    else if (!string.IsNullOrEmpty(model.HtmlButton))
                    {
                        response = client.ConvertHtml(model.Html);
                    }
                    else if (!string.IsNullOrEmpty(model.DocumentButton) && model.Document1.ContentLength > 0)
                    {
                        response = client.ConvertFiles(model.Document1.InputStream, model.Document1.FileName, model.Document2.InputStream, model.Document2.FileName);
                    }
                    else if (!string.IsNullOrEmpty(model.ImageButton) && model.Image.ContentLength > 0)
                    {
                        ImageConversionOptions options = new ImageConversionOptions();

                        int imageWidth;
                        int imageHeight;
                        if (int.TryParse(model.ImageWidth, out imageWidth))
                        {
                            options.ImageWidthPixelsMin = imageWidth;
                        }

                        if (int.TryParse(model.ImageHeight, out imageHeight))
                        {
                            options.ImageHeightPixelsMin = imageHeight;
                        }

                        response = client.ConvertImage(model.Image.InputStream, model.Image.FileName, options);
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
