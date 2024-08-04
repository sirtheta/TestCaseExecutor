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
using TestCaseExecutor.MainClasses;
using static TestCaseExecutor.Logic.GeneratePDFReport;

namespace TestCaseExecutor.Report
{
    internal static class ReportTestCaseBuilder
    {
        private static TableBuilder? _table;
        internal static void Build(SectionBuilder section, TestCase testCase)
        {
            CreateTable(section);
            AddTestCaseRow(testCase);
            AddTestStepDescription();

            foreach (var testStep in testCase.TestSteps)
            {
                AddTestStepRow(testStep);
            }
        }

        private static void CreateTable(SectionBuilder section)
        {
            _table = section.AddTable()
                             .SetWidth(XUnit.FromPercent(100))
                             .AddColumnPercentToTable("", 8)
                             .AddColumnPercentToTable("", 29)
                             .AddColumnPercentToTable("", 29)
                             .AddColumnPercentToTable("", 29)
                             .AddColumnPercentToTable("", 5);
        }

        private static void AddTestStepDescription()
        {
            _table.AddRow()
                    .AddCellToRow()
                    .SetFont(FNT10B)
                .AddCellToRow("Testbeschreibung")
                    .SetFont(FNT10B)
                    .SetHorizontalAlignment(HorizontalAlignment.Center)
                .AddCellToRow("Erwartetes Ergebnis")
                    .SetFont(FNT10B)
                    .SetHorizontalAlignment(HorizontalAlignment.Center)
                .AddCellToRow("Benutzertext")
                    .SetFont(FNT10B)
                    .SetHorizontalAlignment(HorizontalAlignment.Center)
                .AddCellToRow();
        }

        private static void AddTestCaseRow(TestCase testCase)
        {
            _table.AddRow()
                    .AddCellToRow(testCase.ID)
                    .SetFont(FNT10B)
                .AddCell(testCase.Title)
                    .SetFont(FNT10B)
                    .SetColSpan(3)
                .ToRow()
                .AddCell()
                    .SetVerticalAlignment(VerticalAlignment.Center)
                    .AddImage(SetIconPath(testCase, null))
                    .SetAlignment(HorizontalAlignment.Center)
                    .SetHeight(20)
                    .SetMargins(1)
                .ToRow();
        }

        private static void AddTestStepRow(TestStep testStep)
        {
            _table.AddRow()
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
