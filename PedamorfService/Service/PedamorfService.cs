using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pedamorf.Library;
using NLog;

namespace Pedamorf.Service
{
    public class PedamorfService : IPedamorfService
    {
        private Logger m_logger;
        private PdfConverter m_converter;

        public PedamorfService()
        {
            m_converter = new PdfConverter();
            m_logger = LogManager.GetCurrentClassLogger();
        }

        public PedamorfResponse ConvertUrl(string url, HtmlConversionOptions options)
        {
            return GetResponse(x => x.ConvertUrl(url, options));
        }

        public PedamorfResponse ConvertUrls(string[] urls, Library.HtmlConversionOptions options)
        {
            return GetResponse(x => x.ConvertUrls(urls, options));
        }

        public PedamorfResponse ConvertText(string text, Library.ConversionOptions options)
        {
            return GetResponse(x => x.ConvertText(text, options));
        }

        public PedamorfResponse ConvertFile(byte[] file, string fileName, Library.ConversionOptions options)
        {
            return GetResponse(x => x.ConvertFile(file, fileName, options));
        }

        public PedamorfResponse ConvertFiles(Dictionary<string, byte[]> files, Library.ConversionOptions options)
        {
            return GetResponse(x => x.ConvertFiles(files, options));
        }

        public PedamorfResponse ConvertImage(byte[] image, string fileName, Library.ImageConversionOptions options)
        {
            return GetResponse(x => x.ConvertImage(image, fileName, options));
        }

        public PedamorfResponse ConvertImages(Dictionary<string, byte[]> images, Library.ImageConversionOptions options)
        {
            return GetResponse(x => x.ConvertImages(images, options));
        }

        public PedamorfResponse ConvertHtml(string html, Library.HtmlConversionOptions options)
        {
            return GetResponse(x => x.ConvertHtml(html, options));
        }

        public PedamorfResponse ConvertHtmlList(string[] html, Library.HtmlConversionOptions options)
        {
            return GetResponse(x => x.ConvertHtmlList(html, options));
        }

        public PedamorfResponse CombinePdfs(List<byte[]> sourcePdfs, Library.ConversionOptions options)
        {
            return GetResponse(x => x.CombinePdfs(sourcePdfs, options));
        }

        public PedamorfResponse ConvertZipFile(byte[] zipFile, Library.ConversionOptions options)
        {
            return GetResponse(x => x.ConvertZipFile(zipFile, options));
        }

        public bool IsFileTypeSupported(string fileName)
        {
            return m_converter.IsFileTypeSupported(fileName);
        }

        public PedamorfResponse GetResponse(Func<PdfConverter, byte[]> conversion)
        {
            PedamorfResponse response = new PedamorfResponse();

            try
            {
                response.ResultPdf = conversion(m_converter);
            }
            catch (MissingSourceException missingSourceEx)
            {
                m_logger.ErrorException("Source is missing or undefinied.", missingSourceEx);

                response.Error = true;
                response.ErrorMessage = missingSourceEx.Message;
                response.ErrorCode = ErrorCodeEnum.UNDEFINED_SOURCE;
            }
            catch (ConversionTimeoutException timeoutEx)
            {
                m_logger.ErrorException("Conversion engine timeout.", timeoutEx);

                response.Error = true;
                response.ErrorMessage = timeoutEx.Message;
                response.ErrorCode = ErrorCodeEnum.TIMEOUT;
            }
            catch (UrlFetchException urlFetchEx)
            {
                m_logger.ErrorException("Url fetch failed.", urlFetchEx);

                response.Error = true;
                response.ErrorMessage = urlFetchEx.Message;
                response.ErrorCode = ErrorCodeEnum.COULD_NOT_FETCH_URL;
            }
            catch (ConversionEngineException conversionEx)
            {
                m_logger.ErrorException("Conversion engine failed.", conversionEx);

                response.Error = true;
                response.ErrorMessage = "Conversion engine failed.";
                response.ErrorCode = ErrorCodeEnum.ENGINE_ERROR;
            }
            catch (UnsupportedSourceException unsupportedException)
            {
                m_logger.ErrorException("Unsupported type.", unsupportedException);

                response.Error = true;
                response.ErrorMessage = unsupportedException.Message;
                response.ErrorCode = ErrorCodeEnum.UNSUPPORTED_SOURCE;
            }
            catch (Exception ex)
            {
                m_logger.ErrorException("Conversion exception occured.", ex);

                response.Error = true;
                response.ErrorMessage = ex.Message;
                response.ErrorCode = ErrorCodeEnum.OTHER;
            }

            return response;
        }
    }
}
