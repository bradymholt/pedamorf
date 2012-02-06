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
    public class TestHtmlConversion
    {
         [TestMethod]
        public void TestNoWrappingTagsHtml()
        {
            string testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\html_raw\\no_wrapping_tags.txt");
            string html = File.ReadAllText(testFilePath);
            TestHtml(html);
        }

        [TestMethod]
        public void TestWithWrappingTagsHtml()
        {
            string testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\html_raw\\with_wrapping_tags.txt");
            string html = File.ReadAllText(testFilePath);
            TestHtml(html);
        }

        [TestMethod]
        public void TestFullHtmlSimple()
        {
            string testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\html\\html_simple.html");
            string html = File.ReadAllText(testFilePath);
            TestHtml(html);
        }

        [TestMethod]
        public void TestFullHtmlComplex()
        {
            string testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\html\\html_complex.html");
            string html = File.ReadAllText(testFilePath);
            TestHtml(html);
        }


        private void TestHtml(string html)
        {
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertHtml(html, new HtmlConversionOptions());
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }
    }
}
