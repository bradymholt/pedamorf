using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Pedamorf.Library;
using Ionic.Zip;
using NLog;
using System.Threading.Tasks;
using System.Threading;

namespace Pedamorf.Library
{
    public abstract class ConversionEngine : IDisposable
    {
        protected const int DEFAULT_TIMEOUT_MILLISECONDS = 10000;

        private List<string> m_sweepFiles;
        protected static Logger s_logger;

        static ConversionEngine()
        {
            s_logger = LogManager.GetCurrentClassLogger();
        }

        public ConversionEngine()
        {
            m_sweepFiles = new List<string>();
        }

        public void AddFileToSweep(string filePath)
        {
            m_sweepFiles.Add(filePath);
        }

        private byte[] m_outputFile;
        public byte[] OutputFile
        {
            get { return m_outputFile; }
            protected set { m_outputFile = value; }
        }

        protected abstract void Convert(ConversionOptions options);

        public void ConvertToPdf(ConversionOptions options)
        {
            Convert(options);
        }

        protected static void EnsureContentUnpacked(string contentPackFile)
        {
            string contentPackageLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin_external", contentPackFile);
            string contentPackageTargetPath = contentPackageLocation.Replace(".zip", string.Empty);

            try
            {
                if (!Directory.Exists(contentPackageTargetPath))
                {
                    s_logger.Debug("Unpacking {0}...", contentPackFile);
                    using (ZipFile zip = ZipFile.Read(contentPackageLocation))
                    {
                        zip.ExtractAll(contentPackageTargetPath);
                    }
                    Console.WriteLine("success!");
                }
            }
            catch (Exception ex)
            {
                s_logger.WarnException("Exception when trying to unpack content file: " + contentPackFile, ex);
            }

        }

        public void Dispose()
        {
            foreach (string fileName in m_sweepFiles)
            {

                if (File.Exists(fileName))
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch (Exception ex)
                    {
                        s_logger.WarnException("Exception when trying to delete sweep file: " + fileName, ex);
                    }
                }

            }
        }
    }
}
