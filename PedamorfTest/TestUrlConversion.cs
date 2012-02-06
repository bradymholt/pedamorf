using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Pedamorf.Library;

namespace PedamorfTest
{
    [TestClass]
    public class TestUrlConversion
    {
        [TestMethod]
        public void TestValidUrl()
        {
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertUrl("http://www.google.com", new HtmlConversionOptions());
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }

        [TestMethod]
        public void TestSecureUrl()
        {
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertUrl("https://www.google.com", new HtmlConversionOptions());
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ConversionEngineException))]
        public void TestInvalidUrl()
        {
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertUrl("https://www.sdfksdlfklsdf.com", new HtmlConversionOptions());
        }


        [TestMethod]
        [ExpectedException(typeof(UnsupportedSourceException))]
        public void TestRelativedUrl()
        {
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertUrl("/webapp/index.php", new HtmlConversionOptions());
        }
    }
}
