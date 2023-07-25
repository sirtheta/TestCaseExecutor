using Newtonsoft.Json;
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
        internal static void SaveTestDataFile(string filePath, TestSuite testCases)
        {
            string jsonString = JsonConvert.SerializeObject(testCases, Formatting.Indented);

            File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Load the testsuite from JSON
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        internal static TestSuite LoadTestDataFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            TestSuite deserializedList = JsonConvert.DeserializeObject<TestSuite>(json)!;
            return deserializedList;

        }
    }
}
