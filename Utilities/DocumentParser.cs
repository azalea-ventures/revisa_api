using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Utils;
public static class PdfDocumentParser
{
    public static readonly String DEST = "results/sandbox/merge/splitDocument1_{0}.pdf";

    public static readonly String RESOURCE = "EM1_TEKS_GK_M1_TE_v2.pdf";

    public static void ParsePdfDocument()
    {
        PdfDocument pdfDoc = new PdfDocument(new PdfReader(RESOURCE));
        var strategy = new LocationTextExtractionStrategy();
        strategy.SetUseActualText(true);
        IList<PdfPage> lessonPages = new List<PdfPage>();
        int numPages = pdfDoc.GetNumberOfPages();

        for (int i = 1; i <= numPages; i++)
        {
            PdfPage page = pdfDoc.GetPage(i);
            PdfDictionary pageDict = page.GetPdfObject();

            List<PdfName> pageKeys = pageDict.KeySet().ToList();


            string pageText = PdfTextExtractor.GetTextFromPage(page, strategy);
            if (pageText.ToLower().Contains("lesson 1"))
            {
                
                using (StreamWriter outputFile = new StreamWriter(Path.Combine("test", "lesson_1_"+i+".txt"))){
                    outputFile.WriteLine(pageText);
                }
                lessonPages.Add(page);
            }
        }

        pdfDoc.Close();
    }

    private class CustomPdfSplitter : PdfSplitter
    {
        private String dest;
        private int partNumber = 1;

        public CustomPdfSplitter(PdfDocument pdfDocument, String dest)
            : base(pdfDocument)
        {
            this.dest = dest;
        }

        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            return new PdfWriter(String.Format(dest, partNumber++));
        }
    }
}
