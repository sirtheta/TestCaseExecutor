using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TestCaseExecutor.MainClasses;

namespace TestCaseExecutor.Logic
{
    internal class SaveAndLoadTestData
    {
        internal static void SaveTestDataFile(string filePath, IList<TestCase> testCases)
        {
            string jsonString = JsonConvert.SerializeObject(testCases);

            File.WriteAllText(filePath, jsonString);
        }

        internal static List<TestCase> LoadTestDataFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<TestCase> deserializedList = JsonConvert.DeserializeObject<List<TestCase>>(json)!;
            return deserializedList;

        }
    }
}
