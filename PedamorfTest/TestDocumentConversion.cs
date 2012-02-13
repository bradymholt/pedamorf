using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Pedamorf.Library;
using iTextSharp.text;
using iTextSharp.text.pdf;

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

        [TestMethod]
        public void TestMultipleDocuments()
        {
            byte[] testFile1 = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.docx"));
            byte[] testFile2 = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\document.docx"));
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            files.Add("document1.docx", testFile1);
            files.Add("document2.docx", testFile2);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertFiles(files, new ConversionOptions());

            Assert.IsNotNull(pdf);
            Document doc = new Document();
            PdfReader reader = new PdfReader(pdf);
            int pages = reader.NumberOfPages;
            byte[] page1 = reader.GetPageContent(1);
            byte[] page2 = reader.GetPageContent(2);
            doc.Close();

            Assert.IsTrue(pages == 2);
            Assert.AreEqual(page1.Length, page1.Length);
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
