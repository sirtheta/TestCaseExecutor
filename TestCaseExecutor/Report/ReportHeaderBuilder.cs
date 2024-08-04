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
using static TestCaseExecutor.Logic.GeneratePDFReport;

namespace TestCaseExecutor.Report
{
    internal static class ReportHeaderBuilder
    {
        internal static void Build(SectionBuilder section, string testSuiteName)
        {
            section.AddHeaderToBothPages(40)
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
