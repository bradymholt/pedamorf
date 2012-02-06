using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using Pedamorf.Library;

namespace PedamorfTest
{
    [TestClass]
    public class TestConcurrentConversion
    {
        [TestMethod]
        public void TestConcurrentImageConversion()
        {
            string[] images = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\images\\"));
            Parallel.ForEach(images, currentFile =>
                {
                    TestImage(currentFile);
                });
        }

        [TestMethod]
        public void TestConcurrentDocumentConversion()
        {
            string[] documents = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\documents\\"));
            Parallel.ForEach(documents, currentFile =>
            {
                TestFile(currentFile);
            });
        }

        private void TestImage(string testFilePath)
        {
            byte[] testFile = File.ReadAllBytes(testFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertImage(testFile, Path.GetFileName(testFilePath), new ImageConversionOptions());
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }

        private void TestFile(string testFilePath)
        {
            byte[] testFile = File.ReadAllBytes(testFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertFile(testFile, Path.GetFileName(testFilePath), new ConversionOptions());
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }
    }
}
