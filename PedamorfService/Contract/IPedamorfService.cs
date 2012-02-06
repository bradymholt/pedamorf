using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Pedamorf.Library;

namespace Pedamorf.Service
{
    [ServiceContract]
    public interface IPedamorfService
    {
        [OperationContract]
        PedamorfResponse ConvertUrl(string url, HtmlConversionOptions options);

        [OperationContract]
        PedamorfResponse ConvertUrls(string[] urls, HtmlConversionOptions options);

        [OperationContract]
        PedamorfResponse ConvertText(string text, ConversionOptions options);

        [OperationContract]
        PedamorfResponse ConvertFile(byte[] file, string fileName, ConversionOptions options);

        [OperationContract]
        PedamorfResponse ConvertFiles(Dictionary<string, byte[]> files, ConversionOptions options);

        [OperationContract]
        PedamorfResponse ConvertImage(byte[] image, string fileName, ImageConversionOptions options);

        [OperationContract]
        PedamorfResponse ConvertImages(Dictionary<string, byte[]> images, ImageConversionOptions options);

        [OperationContract]
        PedamorfResponse ConvertHtml(string html, HtmlConversionOptions options);

        [OperationContract]
        PedamorfResponse ConvertHtmlList(string[] html, HtmlConversionOptions options);

        [OperationContract]
        PedamorfResponse CombinePdfs(List<byte[]> sourcePdfs, ConversionOptions options);

        [OperationContract]
        PedamorfResponse ConvertZipFile(byte[] zipFile, ConversionOptions options);

        [OperationContract]
        bool IsFileTypeSupported(string fileName);
    }
}
