using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;

namespace Pedamorf.Service.Client
{
    public partial class PedamorfServiceClient : IDisposable
    {
        public PedamorfResponse ConvertUrl(string url) { return ConvertUrl(url, new HtmlConversionOptions()); }
        public PedamorfResponse ConvertUrl(List<string> urls) { return ConvertUrls(urls, new HtmlConversionOptions()); }
        public PedamorfResponse ConvertHtml(string html) { return ConvertHtml(html, new HtmlConversionOptions()); }
        public PedamorfResponse ConvertHtmlList(List<string> html) { return ConvertHtmlList(html, new HtmlConversionOptions()); }
        public PedamorfResponse ConvertText(string text) { return ConvertText(text, new ConversionOptions()); }
        public PedamorfResponse ConvertFile(byte[] file, string fileName) { return ConvertFile(file, fileName, new ConversionOptions()); }
        public PedamorfResponse ConvertFiles(Dictionary<string, byte[]> files) { return ConvertFiles(files, new ConversionOptions()); }
        public PedamorfResponse ConvertImage(byte[] image, string fileName) { return ConvertImage(image, fileName, new ImageConversionOptions()); }
        public PedamorfResponse ConvertImages(Dictionary<string, byte[]> images) { return ConvertImages(images, new ImageConversionOptions()); }
        public PedamorfResponse CombinePdfs(List<byte[]> sourcePdfs) { return CombinePdfs(sourcePdfs, new ConversionOptions()); }
        public PedamorfResponse ConvertZipFile(byte[] zipFile) { return ConvertZipFile(zipFile, new ConversionOptions()); }

        public PedamorfResponse ConvertFile(string filePath) { return ConvertFile(filePath, new ConversionOptions()); }
        public PedamorfResponse ConvertFile(string filePath, ConversionOptions options)
        {
            byte[] file = File.ReadAllBytes(filePath);
            return ConvertFile(file, Path.GetFileName(filePath), options);
        }

        public PedamorfResponse ConvertFile(Stream fileSteam, string fileName) { return ConvertFile(fileSteam, fileName, new ConversionOptions()); }
        public PedamorfResponse ConvertFile(Stream fileSteam, string fileName, ConversionOptions options)
        {
            byte[] file = GetStreamFile(fileSteam);
            return ConvertFile(file, fileName, options);
        }

        public PedamorfResponse ConvertFile(Stream fileSteam1, string fileName1, Stream fileStream2, string fileName2, ConversionOptions options)
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            byte[] file1 = GetStreamFile(fileSteam1);
            byte[] file2 = GetStreamFile(fileSteam1);
            files.Add(fileName1, file1);
            files.Add(fileName2, file2);
            return ConvertFiles(files);
        }

        public PedamorfResponse ConvertFiles(string directoryPath) { return ConvertFiles(directoryPath, new ConversionOptions()); }
        public PedamorfResponse ConvertFiles(string directoryPath, ConversionOptions options)
        {
            Dictionary<string, byte[]> files = GetDirectoryFiles(directoryPath);
            return ConvertFiles(files, options);
        }

        public PedamorfResponse ConvertImage(string filePath) { return ConvertImage(filePath, new ImageConversionOptions()); }
        public PedamorfResponse ConvertImage(string filePath, ImageConversionOptions options)
        {
            byte[] file = File.ReadAllBytes(filePath);
            return ConvertImage(file, Path.GetFileName(filePath), options);
        }

        public PedamorfResponse ConvertImage(Stream fileSteam, string fileName) { return ConvertImage(fileSteam, fileName, new ImageConversionOptions()); }
        public PedamorfResponse ConvertImage(Stream fileSteam, string fileName, ImageConversionOptions options)
        {
            byte[] file = GetStreamFile(fileSteam);
            return ConvertImage(file, fileName, options);
        }

        public PedamorfResponse ConvertImages(string directoryPath) { return ConvertImages(directoryPath, new ImageConversionOptions()); }
        public PedamorfResponse ConvertImages(string directoryPath, ImageConversionOptions options)
        {
            Dictionary<string, byte[]> files = GetDirectoryFiles(directoryPath);
            return ConvertImages(files, options);
        }

        private Dictionary<string, byte[]> GetDirectoryFiles(string directoryPath)
        {
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            string[] directoryFiles = Directory.GetFiles(directoryPath);
            foreach (string fileName in directoryFiles)
            {
                byte[] file = File.ReadAllBytes(Path.Combine(directoryPath, fileName));
                files.Add(fileName, file);
            }
            return files;
        }

        private byte[] GetStreamFile(Stream input)
        {
            byte[] file = null;
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                file = ms.ToArray();
            }

            return file;
        }

        public void Dispose()
        {
            if (this.State == CommunicationState.Faulted)
            {
                this.Abort();
            }
            else
            {
                this.Close();
            }
        }
    }
}
