using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace Pedamorf.Library
{
    class DefaultEngineFactory : EngineFactory
    {
        public override ConversionEngine ConstructUrlEngine(string[] urls)
        {
            return new WebKitRemoteEngine(urls);
        }

        public override ConversionEngine ConstructHtmlEngine(string[] html)
        {
            return new WebKitHtmlEngine(html);
        }

        public override ConversionEngine ConstructTextEngine(string text)
        {
            return new WebKitTextEngine(text);
        }

        public override ConversionEngine ConstructPdfEngine(List<byte[]> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new MissingSourceException("Pdf not specified.");
            }

            return new PdfEngine(files);
        }

        public override bool IsFileTypeSupported(string fileName)
        {
            return !(ConstructFileEngine(fileName, null, UnsupportedTypeHandlingEnum.NoPage) is NullEngine);
        }

        public override ConversionEngine ConstructFileEngine(string fileName, byte[] file,
            UnsupportedTypeHandlingEnum unsupportedHandling)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new MissingSourceException("File not specified.");
            }

            ConversionEngine engine = null;
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".pdf":
                    engine = new PdfEngine(file);
                    break;
                case ".png":
                case ".gif":
                case ".jpeg":
                case ".jpg":
                case ".bmp":
                    engine = new WebKitImageEngine(fileName, file);
                    break;
                case ".html":
                case ".htm":
                    engine = new WebKitFileEngine(fileName, file);
                    break;
                case ".odt":
                case ".doc":
                case ".docx":
                case ".xls":
                case ".xlsx":
                case ".ppt":
                case ".pptx":
                case ".txt":
                case ".rtf":
                    engine = new LibreOfficeConversionEngine(fileName, file);
                    break;
                default:
                    switch (unsupportedHandling)
                    {
                        case UnsupportedTypeHandlingEnum.Error:
                            throw new UnsupportedSourceException("Unsupported file type.", Path.GetExtension(fileName));
                        case UnsupportedTypeHandlingEnum.NoPage:
                            engine = new NullEngine();
                            break;
                        case UnsupportedTypeHandlingEnum.PageWithErrorText:
                            engine = new UnsupportedTypeEngine(fileName);
                            break;
                        default:
                            throw new InvalidEnumArgumentException("unsupportedHandling", (int)unsupportedHandling, typeof(UnsupportedTypeHandlingEnum));
                    }
                    break;
            }

            return engine;
        }
    }
}
