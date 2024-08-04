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
using Gehtsoft.PDFFlow.Utils;
using System.Linq;
using TestCaseExecutor.MainClasses;
using TestCaseExecutor.Report;

namespace TestCaseExecutor.Logic
{
    internal static class GeneratePDFReport
    {
        // font defines
        internal static readonly FontBuilder FNT8 = Fonts.Helvetica(8f);
        internal static readonly FontBuilder FNT10 = Fonts.Helvetica(10f);
        internal static readonly FontBuilder FNT10B = Fonts.Helvetica(10f).SetBold();
        internal static readonly FontBuilder FNT19B = Fonts.Helvetica(19f).SetBold();

        /// <summary>
        /// Main function to generate the report
        /// </summary>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        internal static DocumentBuilder Build(TestSuite testSuite)
        {
            var document = DocumentBuilder.New();
            var section = document.AddSection();
            section.SetOrientation(PageOrientation.Portrait)
                   .SetSize(PaperSize.A4)
                   .SetMargins(29, 21, 29, 12);

            // Generate the header for the document
            ReportHeaderBuilder.Build(section, testSuite.TestSuiteName);

            // get last test case
            var lastTestCase = testSuite.TestCases.Last();

            foreach (var testCase in testSuite.TestCases)
            {
                // generate table in document for each testCase
                // and each teststep in testcase
                //ReportTestCaseBuilder builder = new();
                ReportTestCaseBuilder.Build(section, testCase);

                // if it is not the last test case, add a spacing line
                if (!testCase.Equals(lastTestCase))
                {
                    ReportSpaceBuilder.Build(section);
                }
            }

            ReportFooterBuilder.Build(section);

            return document;
        }

        /// <summary>
        /// returns the icon path for the success or failed icon
        /// use either testCase or testStep, depending on what is needed
        /// one parameter must be null
        /// </summary>
        /// <param name="testCase"></param>
        /// <param name="testStep"></param>
        /// <returns>string for the icon</returns>
        internal static string SetIconPath(TestCase? testCase = null, TestStep? testStep = null)
        {
            bool? success = null;

            if (testCase?.AllStepsSuccessfully == true)
            {
                success = true;
            }
            else if (testCase?.AllStepsSuccessfully == false)
            {
                success = false;
            }

            if (testStep?.TestStepSuccess == true)
            {
                success = true;
            }
            else if (testStep?.TestStepSuccess == false)
            {
                success = false;
            }

            return success == true ? "resources\\success.png" : (success == false ? "resources\\failed.png" : "resources\\notExecuted.png");
        }
    }
}
