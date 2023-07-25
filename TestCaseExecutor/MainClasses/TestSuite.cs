using System.Collections.ObjectModel;
using TestCaseExecutor.Common;

namespace TestCaseExecutor.MainClasses
{
    internal class TestSuite : Notify
    {
        public string TestSuiteName 
        { 
            get=> _testSuiteName; 
            set
            {
                _testSuiteName = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TestCase> TestCases { get; set; } = new();

        private string _testSuiteName = "Test case executor"; 
    }
}
