using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using System;
using static TestCaseExecutor.Logic.GeneratePDFReport;


namespace TestCaseExecutor.Report
{
    internal static class ReportFooterBuilder
    {
        internal static void Build(SectionBuilder section)
        {
            section.AddFooterToBothPages(30)
                   .AddTable()
                   .SetMarginTop(10)
                       .SetWidth(XUnit.FromPercent(100))
                       .AddColumnPercentToTable("", 33)
                       .AddColumnPercentToTable("", 33)
                       .AddColumnPercentToTable("", 34)
                       .AddRow()
                           .SetBorderWidth(0, (float)0.5, 0, 0)
                           .SetFont(FNT8)
                           .AddCellToRow("Generated with Test case executor")
                           .AddCell(DateTime.Now.ToString())
                               .SetHorizontalAlignment(HorizontalAlignment.Center)
                       .ToRow()
                           .AddCell()
                               .SetHorizontalAlignment(HorizontalAlignment.Right)
                               .AddParagraph()
                                   .AddPageNumberToParagraph("Page ");
        }
    }
}
