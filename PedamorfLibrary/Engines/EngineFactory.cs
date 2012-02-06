using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace Pedamorf.Library
{
    public abstract class EngineFactory
    {
        public abstract ConversionEngine ConstructFileEngine(string fileName, byte[] file, UnsupportedTypeHandlingEnum unsupportedHandling);
        public abstract ConversionEngine ConstructUrlEngine(string[] urls);
        public abstract ConversionEngine ConstructHtmlEngine(string[] html);
        public abstract ConversionEngine ConstructTextEngine(string text);
        public abstract ConversionEngine ConstructPdfEngine(List<byte[]> files);
        public abstract bool IsFileTypeSupported(string fileName);
    }
}
