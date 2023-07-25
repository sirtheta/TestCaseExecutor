using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using static TestCaseExecutor.Logic.GeneratePDFReport;

namespace TestCaseExecutor.Report
{
    internal static class ReportHeaderBuilder
    {
        internal static void Build(SectionBuilder section, string testSuiteName)
        {
            section
                .AddHeaderToBothPages(40)
                    .AddTable()
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnToTable()
                    .AddColumnToTable()
                    .AddRow()
                        .AddCell().SetVerticalAlignment(VerticalAlignment.Center)
                            .AddParagraph(testSuiteName + " Testsuite")
                            .SetFont(FNT19B)                            
                    .ToRow()
                    .AddCell()
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddImage("resources\\logo.png")
                    .SetHeight(35)
                    .SetAlignment(HorizontalAlignment.Right)
                    .ToRow()
                    .SetBorderWidth(0, 0, 0, (float)0.5);
        }
    }
}
