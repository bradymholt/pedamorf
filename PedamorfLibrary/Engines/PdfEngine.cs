using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Pedamorf.Library
{
    class PdfEngine : ConversionEngine
    {
        private List<byte[]> m_sourceFiles;
        private bool m_forceConversion;

        public PdfEngine(byte[] file)
            : this(file, false) { }

        public PdfEngine(byte[] file, bool forceSingleFileConversion)
        {
            m_sourceFiles = new List<byte[]>() { file };
            m_forceConversion = forceSingleFileConversion;
        }

        public PdfEngine(List<byte[]> files)
        {
            m_sourceFiles = files;
        }

        protected override void Convert(ConversionOptions options)
        {
            if (m_sourceFiles != null)
            {
                if (m_sourceFiles.Count == 1 && !m_forceConversion)
                {
                    this.OutputFile = m_sourceFiles[0];
                }
                else
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (Document doc = new Document(options.Orientation == PageOrientation.Portrait ? PageSize.LETTER : PageSize.LETTER_LANDSCAPE))
                            {
                                PdfCopy copy = new PdfCopy(doc, ms);
                                PdfReader reader;
                                doc.Open();
                                int n;
                                foreach (byte[] file in m_sourceFiles)
                                {
                                    if (file != null)
                                    {
                                        reader = new PdfReader(file);
                                        n = reader.NumberOfPages;
                                        for (int page = 1; page <= n; page++)
                                        {
                                            copy.AddPage(copy.GetImportedPage(reader, page));
                                        }
                                        copy.FreeReader(reader);
                                    }
                                }
                            }

                            this.OutputFile = ms.ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ConversionEngineException("Error when merging Pdf files.", null, "pdf", this, ex);
                    }
                }
            }
        }
    }
}
