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
    public class TestHtmlFileConversion
    {
        [TestMethod]
        public void TestFullHtmlSimple()
        {
            TestHtml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\html\\html_simple.html"));
        }

        [TestMethod]
        public void TestFullHtmlComplex()
        {
            TestHtml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\html\\html_complex.html"));
        }

        [TestMethod]
        [ExpectedException(typeof(MissingSourceException))]
        public void TestNull()
        {
            PdfConverter converter = new PdfConverter();
            byte[] response = converter.ConvertHtml(null, new HtmlConversionOptions());
        }

        [TestMethod]
        [ExpectedException(typeof(MissingSourceException))]
        public void TestEmpty()
        {
            PdfConverter converter = new PdfConverter();
            byte[] response = converter.ConvertHtml(string.Empty, new HtmlConversionOptions());
        }

        private void TestHtml(string testFilePath)
        {
            byte[] testFile = File.ReadAllBytes(testFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertFile(testFile, Path.GetFileName(testFilePath), new ConversionOptions());
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }
    }
}
