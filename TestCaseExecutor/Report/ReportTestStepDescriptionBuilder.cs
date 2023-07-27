using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Shared;
using static TestCaseExecutor.Logic.GeneratePDFReport;

namespace TestCaseExecutor.Report
{
    internal static class ReportTestStepDescriptionBuilder
    {
        internal static void Build(SectionBuilder section)
        {
            section.AddTable()
                    .SetWidth(XUnit.FromPercent(100))
                    .AddColumnPercentToTable("", 5)
                    .AddColumnPercentToTable("", 30)
                    .AddColumnPercentToTable("", 30)
                    .AddColumnPercentToTable("", 30)
                    .AddColumnPercentToTable("", 5)
                    .AddRow()
                        .AddCellToRow()
                        .SetFont(FNT10BI)
                    .AddCellToRow("Testbeschreibung")
                        .SetFont(FNT10BI)
                    .AddCellToRow("Erwartetes Ergebnis")
                        .SetFont(FNT10BI)
                    .AddCellToRow("Benutzertext")
                        .SetFont(FNT10BI)
                    .AddCellToRow();
        }
    }
}
