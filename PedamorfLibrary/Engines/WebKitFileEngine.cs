using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pedamorf.Library
{
    public class WebKitFileEngine : WebKitEngine
    {
        protected Dictionary<string, byte[]> m_sourceFiles;

        public WebKitFileEngine(string fileName, byte[] file)
        {
            m_sourceFiles = new Dictionary<string, byte[]>() { { fileName, file } };
        }

        public WebKitFileEngine(Dictionary<string, byte[]> files)
        {
            m_sourceFiles = files;
        }

        protected WebKitFileEngine() { }

        protected override void Convert(ConversionOptions options)
        {
            List<string> sourceFiles = new List<string>();
            foreach (KeyValuePair<string, byte[]> sourceFile in m_sourceFiles)
            {
                string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(sourceFile.Key));
                File.WriteAllBytes(tempFile, sourceFile.Value);
                sourceFiles.Add(tempFile);
                AddFileToSweep(tempFile);
            }

            ConvertToPdf(sourceFiles.ToArray(), options);
        }
    }
}
