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
    public class TestDocumentConversion
    {
        [TestMethod]
        public void TestDoc()
        {
            TestDocument(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.doc"));
        }

        [TestMethod]
        public void TestDocx()
        {
            TestDocument(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.docx"));
        }

        [TestMethod]
        public void TestOdt()
        {
            TestDocument(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.odt"));
        }

        [TestMethod]
        public void TestPpt()
        {
            TestDocument(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.ppt"));
        }

        [TestMethod]
        public void TestPptx()
        {
            TestDocument(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.pptx"));
        }

        [TestMethod]
        public void TestRtf()
        {
            TestDocument(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.rtf"));
        }

        [TestMethod]
        public void TestTxt()
        {
            TestDocument(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.txt"));
        }

        [TestMethod]
        public void TestXls()
        {
            TestDocument(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.xls"));
        }

        [TestMethod]
        public void TestXlsx()
        {
            TestDocument(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.xlsx"));
        }

        [TestMethod]
        public void TestIsFileTypeNotSupported()
        {
            PdfConverter converter = new PdfConverter();
            Assert.IsFalse(converter.IsFileTypeSupported("unsupportedFileType.exe"));
        }

        [TestMethod]
        public void TestIsFileTypeSupported()
        {
            PdfConverter converter = new PdfConverter();
            Assert.IsTrue(converter.IsFileTypeSupported("supportedFileType.bmp"));
        }

        [TestMethod]
        [ExpectedException(typeof(MissingSourceException))]
        public void TestNull()
        {
            PdfConverter converter = new PdfConverter();
            byte[] response = converter.ConvertFile(null, "nullfile.doc", new ConversionOptions());
        }

        private void TestDocument(string testFilePath)
        {
            byte[] testFile = File.ReadAllBytes(testFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertFile(testFile, Path.GetFileName(testFilePath), new ConversionOptions());
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }
    }
}
