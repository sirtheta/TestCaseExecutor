using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Gehtsoft.PDFFlow.Utils;
using System.Linq;
using TestCaseExecutor.MainClasses;
using TestCaseExecutor.Report;

namespace TestCaseExecutor.Logic
{
    internal class GeneratePDFReport
    {
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8f);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT10BI = Fonts.Helvetica(10f)
            .SetBold().SetOblique();
        internal static readonly FontBuilder FNT12B = Fonts.Helvetica(12f).SetBold();
        internal static readonly FontBuilder FNT13 = Fonts.Helvetica(13f);
        internal static readonly FontBuilder FNT19B = Fonts.Helvetica(19f).SetBold();

        internal static Color DARKBLUE = Color.FromHtml("#000066");

        internal DocumentBuilder Build(TestSuite testSuite)
        {
            var document = DocumentBuilder.New();
            var section = document.AddSection();
            section.SetOrientation(PageOrientation.Portrait)
                   .SetSize(PaperSize.A4)
                   .SetMargins(29, 21, 29, 12);


            ReportHeaderBuilder.Build(section, testSuite.TestSuiteName);

            foreach (var item in testSuite.TestCases)
            {
                ReportTestCaseBuilder.Build(section, item);
            }

            ReportFooterBuilder.Build(section);

            return document;
        }
    }
}
