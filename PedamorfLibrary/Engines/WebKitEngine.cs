using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Pedamorf.Library;
using System.IO;
using System.Configuration;
using System.Net;

namespace Pedamorf.Library
{
    public abstract class WebKitEngine : ConversionEngine
    {
        static WebKitEngine()
        {
            Initialize();
        }

        private const string WKHTMLTOPDF_RELATIVE_PATH = @".\bin_external\wkhtmltopdf\wkhtmltopdf.exe";
        private static object s_initializeLocker = new object();

        protected void ConvertToPdf(string[] urls, ConversionOptions options)
        {
            string outputFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".pdf");
            AddFileToSweep(outputFile);

            Process conversionProcess = new Process();
            conversionProcess.StartInfo.UseShellExecute = false;
            conversionProcess.StartInfo.ErrorDialog = false;
            conversionProcess.StartInfo.CreateNoWindow = true;
            conversionProcess.StartInfo.RedirectStandardOutput = true;
            conversionProcess.StartInfo.RedirectStandardError = true;
            conversionProcess.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            conversionProcess.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, WKHTMLTOPDF_RELATIVE_PATH);

            string optionArguments = GenerateOptionsArguments(options);
            conversionProcess.StartInfo.Arguments = optionArguments;
            conversionProcess.StartInfo.Arguments += " " + string.Join(" ", urls);
            conversionProcess.StartInfo.Arguments += " " + outputFile;

            string standardErrorOutput = string.Empty;

            try
            {
                s_logger.Debug(string.Format("Starting process: {0} with arguments: {1}", WKHTMLTOPDF_RELATIVE_PATH, conversionProcess.StartInfo.Arguments));
                conversionProcess.Start();
                standardErrorOutput = conversionProcess.StandardError.ReadToEnd();
                int timeoutMilliseconds = options.TimeoutInSeconds > 0 ? options.TimeoutInSeconds * 1000 : DEFAULT_TIMEOUT_MILLISECONDS;
                if (!conversionProcess.WaitForExit(timeoutMilliseconds))
                {
                    conversionProcess.Kill();
                    throw new ConversionTimeoutException(string.Format("Conversion timeout after {0} milliseconds.", timeoutMilliseconds.ToString()), standardErrorOutput, "Url", this);
                }
                else if (conversionProcess.ExitCode != 0)
                {
                    if (standardErrorOutput.Contains("Failed loading page"))
                    {
                        throw new UrlFetchException(urls, standardErrorOutput, this);

                    }
                    else
                    {
                        throw new ConversionEngineException("Error when running conversion process: " + WKHTMLTOPDF_RELATIVE_PATH, standardErrorOutput, "Url", this);
                    }
                }

                if (!File.Exists(outputFile))
                {
                    throw new ConversionEngineException("Output Pdf file missing after conversion.", standardErrorOutput, "Url", this);
                }
                else
                {
                    this.OutputFile = File.ReadAllBytes(outputFile);
                }
            }
            catch (Exception ex)
            {
                throw new ConversionEngineException("Error when converting Pdf with process: " + WKHTMLTOPDF_RELATIVE_PATH, standardErrorOutput, "Url", this, ex);
            }
        }

        protected virtual string GenerateOptionsArguments(ConversionOptions options)
        {
            StringBuilder arguments = new StringBuilder();

            if (options != null)
            {
                arguments.Append("--orientation ").Append(options.Orientation.ToString());

                if (options is HtmlConversionOptions)
                {
                    HtmlConversionOptions htmlOptions = (HtmlConversionOptions)options;
                    if (!string.IsNullOrEmpty(htmlOptions.HTTPAuthenticationUsername))
                    {
                        arguments.Append(" --username ").Append(htmlOptions.HTTPAuthenticationUsername);
                        arguments.Append(" --password ").Append(htmlOptions.HTTPAuthenticationPassword);
                    }

                    if (htmlOptions.PrintMediaType)
                    {
                        arguments.Append(" --print-media-type");
                    }

                    if (!string.IsNullOrEmpty(htmlOptions.UserStyleSheetUrl))
                    {
                        arguments.Append(" --user-style-sheet ").Append(htmlOptions.UserStyleSheetUrl);
                    }
                }
            }

            return arguments.ToString();
        }

        public static void Initialize()
        {
            lock (s_initializeLocker)
            {
                EnsureContentUnpacked("wkhtmltopdf.zip");
            }
        }
    }
}
