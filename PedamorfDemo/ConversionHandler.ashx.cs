using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Pedamorf.Service.Client;

namespace PedamorfDemo
{
    public class ConversionHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            PedamorfResponse output = null;
            string errorOutput = "Error!";
            string filename = "converted.pdf";

            using (PedamorfServiceClient client = ServiceManager.GetClient(context.Request.QueryString["host"]))
            {
                if (!string.IsNullOrEmpty(context.Request.QueryString["url"]))
                {
                    output = client.ConvertUrl(context.Request.QueryString["url"], new HtmlConversionOptions());
                    filename = string.Concat(context.Request.QueryString["url"].Replace(".", "_").Replace(":", "_"), ".pdf");

                }
                else if (!string.IsNullOrEmpty(context.Request.QueryString["html"]))
                {
                    output = client.ConvertHtml(context.Request.QueryString["html"], new HtmlConversionOptions());
                    filename = "raw.pdf";
                }
                else if (!string.IsNullOrEmpty(context.Request.QueryString["file"]))
                {
                    output = client.ConvertFile(((byte[])context.Session["fileUploadData"]), (string)context.Session["fileUploadName"], new ConversionOptions());
                    string inputFileName = ((string)context.Session["fileUploadName"]);
                    filename = string.Concat(inputFileName.Substring(0, inputFileName.IndexOf(".")), ".pdf");
                }
                else
                {
                    errorOutput = "Error - invalid query parameters!";
                }
            }

            if (output != null)
            {
                context.Response.Clear();
                context.Response.AddHeader("content-disposition", string.Format("{0}; filename={1}",
                    string.IsNullOrEmpty(context.Request.QueryString["download"]) ? "inline" : "attachment", filename));
                context.Response.ContentType = "application/pdf";
                context.Response.BinaryWrite(output.ResultPdf);
                context.Response.End();
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(errorOutput);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}