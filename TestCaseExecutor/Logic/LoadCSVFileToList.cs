using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TestCaseExecutor.MainClasses;

namespace TestCaseExecutor.Logic
{
    internal class LoadCSVFileToList
    {
        internal IList<TestCase> LoadCSVFile(string fileName)
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

            List<TestCase> testCases = new();
            foreach (var mapping in csvMapping)
            {
                // Check if it's a TestCase object
                if (mapping.ID != string.Empty && mapping.Title != string.Empty)
                {
                    var testCase = new TestCase
                    {
                        ID = mapping.ID,
                        Title = mapping.Title,
                        TestSteps = new List<TestStep>()
                    };

                    testCases.Add(testCase);
                }
                // Check if it's a TestStep object
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

            return testCases;
        }
    }
}
