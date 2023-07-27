using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using TestCaseExecutor.MainClasses;
using static TestCaseExecutor.Logic.GeneratePDFReport;

namespace TestCaseExecutor.Report
{
    internal static class ReportTestStepBuilder
    {
        internal static void Build(SectionBuilder section, TestStep testStep)
        {
            section.AddTable()
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("", 5)
                    .AddColumnPercentToTable("", 30)
                    .AddColumnPercentToTable("", 30)
                    .AddColumnPercentToTable("", 30)
                    .AddColumnPercentToTable("", 5)
                    .AddRow()
                        .AddCellToRow(testStep.TestStepID)
                        .SetFont(FNT10)
                    .AddCellToRow(testStep.StepAction)
                        .SetFont(FNT10)
                    .AddCellToRow(testStep.StepExpected)
                        .SetFont(FNT10)
                    .AddCellToRow(testStep.AdditionalUserText)
                        .SetFont(FNT10)
                    .AddCell()
                        .SetVerticalAlignment(VerticalAlignment.Center)
                        .AddImage(SetIconPath(null, testStep))
                        .SetAlignment(HorizontalAlignment.Center)
                        .SetHeight(20)
                        .SetMargins(1)
                    .ToRow();
        }
    }
}
