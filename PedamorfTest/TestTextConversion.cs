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
    public class TestTextConversion
    {
        [TestMethod]
        public void TestSimpleText()
        {
            TestText("This is a text.  Only a test.");
        }

        [TestMethod]
        public void TestTextWithLineBreaks()
        {
            TestText("This is a text.\n Only a test.\nTesting.");
        }


        private void TestText(string text)
        {
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertText(text, new ConversionOptions());
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }
    }
}
