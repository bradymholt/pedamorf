using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pedamorf.Library
{
    public class WebKitImageEngine : WebKitFileEngine
    {
        public WebKitImageEngine(string fileName, byte[] file)
            : base(fileName, file) { }

        public WebKitImageEngine(Dictionary<string, byte[]> files)
            : base(files) { }

        protected override void Convert(ConversionOptions options)
        {
            List<string> sourceFiles = new List<string>();

            foreach (KeyValuePair<string, byte[]> sourceFile in m_sourceFiles)
            {
                string tempImageFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(sourceFile.Key));
                File.WriteAllBytes(tempImageFile, sourceFile.Value);
                AddFileToSweep(tempImageFile);

                string widthAttribute = string.Empty;
                string heightAttribute = string.Empty;

                if (options != null && options is ImageConversionOptions)
                {
                    ImageConversionOptions imageOptions = (ImageConversionOptions)options;

                    if (imageOptions.ImageWidthPixelsMin.HasValue)
                    {
                        widthAttribute = string.Format("width='{0}'", imageOptions.ImageWidthPixelsMin.Value);
                    }


                    if (imageOptions.ImageHeightPixelsMin.HasValue)
                    {
                        heightAttribute = string.Format("width='{0}'", imageOptions.ImageHeightPixelsMin.Value);
                    }
                }

                string tempHtmlFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
                string tempHtmlFileContent = string.Format("<html><body><img src='{0}' {1} {2} /></body></html>",
                    tempImageFile, widthAttribute, heightAttribute);
                File.WriteAllText(tempHtmlFile, tempHtmlFileContent);
                sourceFiles.Add(tempHtmlFile);
                AddFileToSweep(tempHtmlFile);
            }

            string optionArguments = GenerateOptionsArguments(options);
            ConvertToPdf(sourceFiles.ToArray(), options);
        }
    }
}
