using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace PedamorfDemo.Controllers
{
    public class DemoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Upload(HttpPostedFileBase upFile)
        {
            if (upFile.ContentLength > 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    upFile.InputStream.CopyTo(ms);
                    Session["fileUploadName"] = upFile.FileName;
                    Session["fileUploadData"] = ms.ToArray();
                }
            }
        }
    }
}
