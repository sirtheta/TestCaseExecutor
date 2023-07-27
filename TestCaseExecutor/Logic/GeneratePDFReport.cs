using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Utils;
using System.Linq;
using TestCaseExecutor.MainClasses;
using TestCaseExecutor.Report;

namespace TestCaseExecutor.Logic
{
    internal class GeneratePDFReport
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
            bool success = false;

            if (testCase != null && testCase.AllStepsSuccessfully)
            {
                success = true;
            }

            if (testStep != null && testStep.TestStepSuccess)
            {
                success = true;
            }

            if (success)
            {
                return "resources\\success.png";
            }
            return "resources\\failed.png";
        }
    }
}
