using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using TestCaseExecutor.MainClasses;
using static TestCaseExecutor.Logic.GeneratePDFReport;

namespace TestCaseExecutor.Report
{
    internal static class ReportTestCaseBuilder
    {
        internal static void Build(SectionBuilder section, TestCase testCase)
        {
            section.AddTable()
                .SetMarginBottom(0)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 10)
                .AddColumnPercentToTable("", 85)
                .AddColumnPercentToTable("", 5)
                .AddRow()
                    .AddCellToRow(testCase.ID)
                    .SetFont(FNT10)
                .AddCellToRow(testCase.Title)
                    .SetFont(FNT10)
                .AddCell()
                    .AddImage(SetIconPath(testCase, null))
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetHeight(20)
                    .SetMargins(1)
                .ToRow();
        }
    }
}
