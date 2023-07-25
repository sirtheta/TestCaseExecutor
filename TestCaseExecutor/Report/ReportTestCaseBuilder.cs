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
                .SetMarginBottom(10)
                .SetWidth(XUnit.FromPercent(100))
                .AddColumnPercentToTable("", 10)
                .AddColumnPercentToTable("", 80)
                .AddColumnPercentToTable("", 10)
                .AddRow()
                    .AddCellToRow(testCase.ID)
                    .SetFont(FNT10)
                .AddCellToRow(testCase.Title)
                    .SetFont(FNT10)
                .AddCell()
                    .AddImage(SetIconPath(testCase))
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetHeight(20)
                    .SetMargins(1)
                .ToRow();
        }

        private static string SetIconPath(TestCase testCase)
        {
            if (testCase.AllStepsSuccessfully)
            {
                return "resources\\success.png";
            }
            return "resources\\failed.png";
        }
    }
}
