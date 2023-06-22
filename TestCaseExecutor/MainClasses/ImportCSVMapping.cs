using CsvHelper.Configuration.Attributes;
using System;

namespace TestCaseExecutor.MainClasses
{
    /// <summary>
    /// Mappingclass of the test suite CSV exported from azure devops
    /// </summary>
    internal class ImportCSVMapping
    {
        [Index(0)]
        public string? ID { get; set; }
        [Index(2)]
        public string? Title { get; set; }
        [Index(3)]
        public string? TestStep { get; set; }
        [Index(4)]
        public string? StepAction { get; set; }
        [Index(5)]
        public string? StepExpected { get; set; }
    }
}
