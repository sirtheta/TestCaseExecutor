using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestCaseExecutor.MainClasses
{

    internal class TestCase
    {
        public string? ID { get; set; }
        public string? Title { get; set; }
        public ObservableCollection<TestStep> TestSteps { get; set; } = new ObservableCollection<TestStep>();
    }
}