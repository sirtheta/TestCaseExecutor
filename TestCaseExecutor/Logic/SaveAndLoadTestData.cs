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
