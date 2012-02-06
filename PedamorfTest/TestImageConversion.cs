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
    public class TestImageConversion
    {
        [TestMethod]
        public void TestBmp()
        {
            TestImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\images\\image.bmp"));
        }

        [TestMethod]
        public void TestGif()
        {
            TestImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\images\\image.gif"));
        }

        [TestMethod]
        public void TestJpeg()
        {
            TestImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\images\\image.jpg"));
        }

        [TestMethod]
        public void TestPng()
        {
            TestImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\images\\image.png"));
        }

        [TestMethod]
        [ExpectedException(typeof(UnsupportedSourceException))]
        public void TestExceptionOnUnsupportedImage()
        {
            TestImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\unsupported\\image.tga"));
        }

        [TestMethod]
        public void TestSilentFailEmptyPageOnUnsupportedImage()
        {
            string testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\unsupported\\image.tga");
            byte[] testFile = File.ReadAllBytes(testFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertImage(testFile, Path.GetFileName(testFilePath), new ImageConversionOptions() {SilentFailOnUnsupportedType = true, UsePlaceholderPageOnUnsupportedType = true });
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }

        [TestMethod]
        public void TestSilentFailNoPageOnUnsupportedImage()
        {
            string testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\unsupported\\image.tga");
            byte[] testFile = File.ReadAllBytes(testFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertImage(testFile, Path.GetFileName(testFilePath), new ImageConversionOptions() { SilentFailOnUnsupportedType = true, UsePlaceholderPageOnUnsupportedType = false });
            Assert.IsNull(pdf);
        }

        [TestMethod]
        public void TestMultipleImages()
        {
            string image1Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\images\\image.png");
            string image2Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_files\\images\\image.jpg");
            byte[] image1 = File.ReadAllBytes(image1Path);
            byte[] image2 = File.ReadAllBytes(image2Path);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertImages(new Dictionary<string, byte[]>() { 
                {  Path.GetFileName(image1Path), image1 },
                {  Path.GetFileName(image2Path), image2 }
            }, new ImageConversionOptions());

            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }

        private void TestImage(string testFilePath)
        {
            byte[] testFile = File.ReadAllBytes(testFilePath);
            PdfConverter converter = new PdfConverter();
            byte[] pdf = converter.ConvertImage(testFile, Path.GetFileName(testFilePath), new ImageConversionOptions());
            Assert.IsNotNull(pdf);
            Assert.IsTrue(pdf.Length > 0);
        }
    }
}
