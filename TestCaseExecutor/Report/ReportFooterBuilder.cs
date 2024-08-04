/*
 * Copyright (C) 2024 Michael Neuhaus
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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
