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

using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using TestCaseExecutor.MainClasses;

namespace TestCaseExecutor.Logic
{
    internal class LoadCSVFileToList
    {
        /// <summary>
        /// Reads the CSV file and maps it with the CSV import mapping class
        /// </summary>
        /// <param name="fileName">The path of the CSV file to load.</param>
        /// <returns></returns>
        private static List<ImportCSVMapping> ReadCSVFile(string fileName)
        {
            List<ImportCSVMapping> csvMapping = new();
            using var reader = new StreamReader(fileName);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null
            };
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    csvMapping.Add(csv.GetRecord<ImportCSVMapping>()!);
                }
            }
            return csvMapping;
        }

        /// <summary>
        /// Load a testsuite from a CSV file and return a list of TestCase
        /// </summary>
        /// <param name="fileName">The path of the CSV file to load.</param>
        /// <returns></returns>
        internal static TestSuite LoadCSVFile(string fileName)
        {
            List<TestCase> testCases = new();
            foreach (var mapping in ReadCSVFile(fileName))
            {
                // Check if it's a TestCase object
                if (mapping.ID != string.Empty && mapping.Title != string.Empty)
                {
                    var testCase = new TestCase
                    {
                        ID = mapping.ID,
                        Title = mapping.Title
                    };

                    testCases.Add(testCase);
                }
                // Check if it's a TestStep object, if it is not a testcas, ist should always be a testStep...
                else if (mapping.ID == string.Empty && mapping.TestStep != string.Empty)
                {
                    var testStep = new TestStep
                    {
                        TestStepID = mapping.TestStep,
                        StepAction = mapping.StepAction,
                        StepExpected = mapping.StepExpected
                    };

                    testCases.LastOrDefault()?.TestSteps.Add(testStep);
                }
            }

            TestSuite suite = new()
            {
                TestSuiteName = fileName?.Split('\\').LastOrDefault()?.Split('_').FirstOrDefault() ?? "Test case executor",
                TestCases = new ObservableCollection<TestCase>(testCases),
            };

            return suite;
        }


        /// <summary>
        /// Update existing test cases and test steps with data from the CSV file.
        /// </summary>
        /// <param name="fileName">The path of the CSV file to load.</param>
        /// <param name="currentTestCases">The list of test cases to update.</param>
        internal static void UpdateTestCasesFromCSV(string fileName, TestSuite currentTestSuite)
        {
            TestCase? currentTestCase = null;

            foreach (var mapping in ReadCSVFile(fileName))
            {
                // Check if it's a TestCase object
                if (mapping.ID != string.Empty && mapping.Title != string.Empty)
                {
                    // Find the matching test case in the current list or create a new one
                    currentTestCase = currentTestSuite.TestCases.FirstOrDefault(tc => tc.ID == mapping.ID);
                    if (currentTestCase == null)
                    {
                        currentTestCase = new TestCase
                        {
                            ID = mapping.ID,
                            Title = mapping.Title
                        };
                        currentTestSuite.TestCases.Add(currentTestCase);
                    }
                    else
                    {
                        // If the test case exists, update its Title
                        currentTestCase.Title = mapping.Title;
                    }
                }
                // Check if it's a TestStep object; it should always be a test step at this point
                else if (mapping.TestStep != string.Empty && currentTestCase != null)
                {
                    // Find the matching test step in the current test case or create a new one
                    var testStepToUpdate = currentTestCase.TestSteps.FirstOrDefault(ts => ts.TestStepID == mapping.TestStep);
                    if (testStepToUpdate == null)
                    {
                        var testStep = new TestStep
                        {
                            TestStepID = mapping.TestStep,
                            StepAction = mapping.StepAction,
                            StepExpected = mapping.StepExpected
                        };
                        currentTestCase.TestSteps.Add(testStep);
                    }
                    else
                    {
                        // If the test step exists, update its properties
                        testStepToUpdate.StepAction = mapping.StepAction;
                        testStepToUpdate.StepExpected = mapping.StepExpected;
                    }
                }
            }
        }
    }
}

