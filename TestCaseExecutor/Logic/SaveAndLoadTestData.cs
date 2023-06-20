using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TestCaseExecutor.MainClasses;

namespace TestCaseExecutor.Logic
{
    internal class SaveAndLoadTestData
    {
        /// <summary>
        /// Saves the current test suite to a JSON
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="testCases"></param>
        internal static void SaveTestDataFile(string filePath, IList<TestCase> testCases)
        {
            string jsonString = JsonConvert.SerializeObject(testCases, Formatting.Indented);

            File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Load the testsuite from JSON
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static List<TestCase> LoadTestDataFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<TestCase> deserializedList = JsonConvert.DeserializeObject<List<TestCase>>(json)!;
            return deserializedList;

        }
    }
}
