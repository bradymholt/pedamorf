using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pedamorf.Library;
using System.Diagnostics;
using System.IO;
using System.Configuration;

namespace Pedamorf.Library
{
    public class LibreOfficeConversionEngine : ConversionEngine
    {
        private static object m_processLock = new object();

        static LibreOfficeConversionEngine()
        {
            Initialize();
        }

        private const string LIBREOFFICE_RELATIVE_PATH = @".\bin_external\libreoffice\LibreOfficePortable.exe";
        private static object s_initializeLocker = new object();

        private byte[] m_sourceFile;
        private string m_sourceFileName;

        public LibreOfficeConversionEngine(string fileName, byte[] file)
        {
            m_sourceFile = file;
            m_sourceFileName = fileName;
        }

        protected override void Convert(ConversionOptions options)
        {
            string inputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(m_sourceFileName));
            File.WriteAllBytes(inputFile, m_sourceFile);
            AddFileToSweep(inputFile);

            Process conversionProcess = new Process();
            conversionProcess.StartInfo.UseShellExecute = false;
            conversionProcess.StartInfo.ErrorDialog = false;
            conversionProcess.StartInfo.CreateNoWindow = true;
            conversionProcess.StartInfo.RedirectStandardOutput = true;
            conversionProcess.StartInfo.RedirectStandardError = true;
            conversionProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(inputFile);
            conversionProcess.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LIBREOFFICE_RELATIVE_PATH);
            conversionProcess.StartInfo.Arguments += "--headless -convert-to pdf " + Path.GetFileName(inputFile);

            string outputFile = Path.Combine(Path.GetDirectoryName(inputFile), string.Concat(Path.GetFileNameWithoutExtension(inputFile), ".pdf"));
            AddFileToSweep(outputFile);

            string standardErrorOutput = string.Empty;

            try
            {
                lock (m_processLock) //only 1 instance of libre office can be running at a time
                {
                    s_logger.Debug(string.Format("Starting process: {0} with arguments: {1}", LIBREOFFICE_RELATIVE_PATH, conversionProcess.StartInfo.Arguments));
                    conversionProcess.Start();
                    standardErrorOutput = conversionProcess.StandardError.ReadToEnd();
                    int timeoutMilliseconds = options.TimeoutInSeconds > 0 ? options.TimeoutInSeconds * 1000 : DEFAULT_TIMEOUT_MILLISECONDS;
                    if (!conversionProcess.WaitForExit(timeoutMilliseconds))
                    {
                        conversionProcess.Kill();
                        throw new ConversionTimeoutException(string.Format("Conversion timeout after {0} milliseconds.", timeoutMilliseconds.ToString()), standardErrorOutput, Path.GetExtension(m_sourceFileName), this);
                    }
                    else if (conversionProcess.ExitCode != 0)
                    {
                        string message = "Error when running conversion process: " + LIBREOFFICE_RELATIVE_PATH;
                        throw new ConversionEngineException(message, standardErrorOutput, Path.GetExtension(m_sourceFileName), this);
                    }
                }

                this.OutputFile = File.ReadAllBytes(outputFile);

                if (conversionProcess.ExitCode != 0)
                {
                    throw new ConversionEngineException("Error when running conversion process: " + LIBREOFFICE_RELATIVE_PATH, standardErrorOutput, Path.GetExtension(m_sourceFileName), this);
                }

                if (!File.Exists(outputFile))
                {
                    throw new ConversionEngineException("Output Pdf file missing after conversion.", standardErrorOutput, Path.GetExtension(m_sourceFileName), this);
                }
                else
                {
                    this.OutputFile = File.ReadAllBytes(outputFile);

                    if (options.Orientation == PageOrientation.Landscape)
                    {
                        //change Orientation
                        using (PdfEngine engine = new PdfEngine(this.OutputFile, true))
                        {
                            engine.ConvertToPdf(options);
                            this.OutputFile = engine.OutputFile;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ConversionEngineException("Error when converting Pdf with process: " + LIBREOFFICE_RELATIVE_PATH, standardErrorOutput, Path.GetExtension(m_sourceFileName), this, ex);
            }
        }

        public static void Initialize()
        {
            lock (s_initializeLocker)
            {
                EnsureContentUnpacked("libreoffice.zip");
            }
        }
    }
}
