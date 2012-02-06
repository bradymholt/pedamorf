using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace Pedamorf.Library
{
    public class PdfConverter
    {
        private EngineFactory m_engineFactory;

        public PdfConverter()
            : this(new DefaultEngineFactory()) { }

        public PdfConverter(EngineFactory engineFactory)
        {
            m_engineFactory = engineFactory;
        }

        public byte[] ConvertUrl(string url, HtmlConversionOptions options)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new MissingSourceException("Url not specified.");
            }

            return ConvertUrls(new string[] { url }, options);
        }

        public byte[] ConvertUrls(string[] urls, HtmlConversionOptions options)
        {
            if (urls == null || urls.Length == 0)
            {
                throw new MissingSourceException("No Urls were specified.");
            }

            foreach (string url in urls)
            {
                if (!IsAbsoluteUrl(url))
                {
                    throw new UnsupportedSourceException("The specified url is not an absolute path.", url.ToString());
                }
            }

            using (ConversionEngine engine = m_engineFactory.ConstructUrlEngine(urls))
            {
                engine.ConvertToPdf(options);
                return engine.OutputFile;
            }
        }

        private bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

        public byte[] ConvertText(string text, ConversionOptions options)
        {
            using (ConversionEngine engine = m_engineFactory.ConstructTextEngine(text))
            {
                engine.ConvertToPdf(options);
                return engine.OutputFile;
            }
        }

        public byte[] ConvertFile(byte[] file, string fileName, ConversionOptions options)
        {
            if (file == null || file.Length == 0)
            {
                throw new MissingSourceException("File not specified.");
            }

            return ConvertFiles(new Dictionary<string, byte[]>() { { fileName, file } }, options);
        }

        public byte[] ConvertFiles(Dictionary<string, byte[]> files, ConversionOptions options)
        {
            if (files == null || files.Count == 0)
            {
                throw new MissingSourceException("No files were specified.");
            }

            List<byte[]> converted = new List<byte[]>();
            foreach (KeyValuePair<string, byte[]> file in files)
            {
                UnsupportedTypeHandlingEnum unsupportedHandling = GetUnsupportedHandlingFromOptions(options);
                using (ConversionEngine engine = m_engineFactory.ConstructFileEngine(file.Key, file.Value, unsupportedHandling))
                {
                    engine.ConvertToPdf(options);
                    byte[] result = engine.OutputFile;
                    converted.Add(result);
                }
            }

            return CombinePdfs(converted, options);
        }

        public byte[] ConvertImage(byte[] image, string fileName, ImageConversionOptions options)
        {
            if (image == null || image.Length == 0)
            {
                throw new MissingSourceException("No image was specified.");
            }

            return ConvertImages(new Dictionary<string, byte[]>() { { fileName, image } }, options);
        }

        public byte[] ConvertImages(Dictionary<string, byte[]> images, ImageConversionOptions options)
        {
            if (images == null || images.Count == 0)
            {
                throw new MissingSourceException("No images were specified.");
            }

            List<byte[]> converted = new List<byte[]>();
            foreach (KeyValuePair<string, byte[]> file in images)
            {
                UnsupportedTypeHandlingEnum unsupportedHandling = GetUnsupportedHandlingFromOptions(options);
                using (ConversionEngine engine = m_engineFactory.ConstructFileEngine(file.Key, file.Value, unsupportedHandling))
                {
                    engine.ConvertToPdf(options);
                    byte[] result = engine.OutputFile;
                    converted.Add(result);
                }
            }

            return CombinePdfs(converted, options);
        }

        public byte[] ConvertHtml(string html, HtmlConversionOptions options)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new MissingSourceException("No html was specified.");
            }

            return ConvertHtmlList(new string[] { html }, options);
        }

        public byte[] ConvertHtmlList(string[] html, HtmlConversionOptions options)
        {
            if (html == null || html.Length == 0)
            {
                throw new MissingSourceException("No html was specified.");
            }

            using (ConversionEngine engine = m_engineFactory.ConstructHtmlEngine(html))
            {
                engine.ConvertToPdf(options);
                return engine.OutputFile;
            }
        }

        public byte[] CombinePdfs(List<byte[]> sourcePdfs, ConversionOptions options)
        {
            if (sourcePdfs == null || sourcePdfs.Count == 0)
            {
                throw new MissingSourceException("No pdfs were specified.");
            }

            using (ConversionEngine engine = m_engineFactory.ConstructPdfEngine(sourcePdfs))
            {
                engine.ConvertToPdf(options);
                return engine.OutputFile;
            }
        }

        public byte[] ConvertZipFile(byte[] zipFile, ConversionOptions options)
        {
            if (zipFile == null || zipFile.Length == 0)
            {
                throw new MissingSourceException("No zip file was specified.");
            }

            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            using (MemoryStream sourceStream = new MemoryStream(zipFile))
            {
                using (ZipFile zip = ZipFile.Read(sourceStream))
                {
                    foreach (ZipEntry e in zip)
                    {
                        using (MemoryStream targetStream = new MemoryStream())
                        {
                            e.Extract(targetStream);
                            files.Add(e.FileName, targetStream.ToArray());
                        }
                    }
                }
            }

            return ConvertFiles(files, options);
        }

        public bool IsFileTypeSupported(string fileName)
        {
            return m_engineFactory.IsFileTypeSupported(fileName);
        }

        private UnsupportedTypeHandlingEnum GetUnsupportedHandlingFromOptions(ConversionOptions options)
        {
            UnsupportedTypeHandlingEnum handling = UnsupportedTypeHandlingEnum.Error;
            if (!options.SilentFailOnUnsupportedType)
            {
                handling = UnsupportedTypeHandlingEnum.Error;
            }
            else if (options.UsePlaceholderPageOnUnsupportedType)
            {
                handling = UnsupportedTypeHandlingEnum.PageWithErrorText;
            }
            else
            {
                handling = UnsupportedTypeHandlingEnum.NoPage;
            }

            return handling;
        }
    }
}
