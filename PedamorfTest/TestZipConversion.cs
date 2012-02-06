using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pedamorf.Library;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PedamorfTest
{
    [TestClass]
    public class TestZipConversion
    {
        [TestMethod]
        public void TestZip()
        {
            string tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\zip\\archive.zip");
            byte[] testFile = File.ReadAllBytes(tempFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertZipFile(testFile, new ConversionOptions());
            Assert.IsNotNull(pdf);

            Document doc = new Document();
            PdfReader reader = new PdfReader(pdf);
            int pages = reader.NumberOfPages;
            doc.Close();

            Assert.IsTrue(pages == 5);
        }

        [TestMethod]
        [ExpectedException(typeof(UnsupportedSourceException))]
        public void TestZipExceptionOnUnsupportedFileType()
        {
            string tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\zip\\archive_with_unsupported_file.zip");
            byte[] testFile = File.ReadAllBytes(tempFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertZipFile(testFile, new ConversionOptions() { SilentFailOnUnsupportedType = false });
        }

        [TestMethod]
        public void TestZipUnsupportedFileType()
        {
            string tempFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\zip\\archive_with_unsupported_file.zip");
            byte[] testFile = File.ReadAllBytes(tempFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertZipFile(testFile, new ConversionOptions() { SilentFailOnUnsupportedType = true, UsePlaceholderPageOnUnsupportedType = true });
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);

            Document doc = new Document();
            PdfReader reader = new PdfReader(pdf);
            int pages = reader.NumberOfPages;
            doc.Close();

            Assert.IsTrue(pages == 6);
        }
    }
}
