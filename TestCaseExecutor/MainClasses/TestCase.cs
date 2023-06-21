using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TestCaseExecutor.Commands;

namespace TestCaseExecutor.MainClasses
{
    /// <summary>
    /// Class for the testcases
    /// </summary>
    internal class TestCase : INotifyPropertyChanged
    {
        public string? ID { get; set; }
        public string? Title { get; set; }
        public List<TestStep> TestSteps { get; set; } = new List<TestStep>();

        private bool _isExpanded = false;
        private bool _allTestStepsExecuted = false;

        public TestCase()
        {
            ToggleExpandCommand = new RelayCommand<object>(ToggleExpand);
        }

        public bool IsExpanded
        {
            get => _isExpanded; 
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public bool AllTestStepsExecuted
        {
            get => _allTestStepsExecuted;
            set
            {
                _allTestStepsExecuted = value;
                OnPropertyChanged();
            }
        }

        public bool AllCheckBoxesClicked()
        {
            return TestSteps.All(step => step.CheckBoxClicked);
        }

        // command to toggle the expand of the testcase
        public ICommand ToggleExpandCommand
        {
            get;
        }

        private void ToggleExpand(object obj)
        {
            IsExpanded = !IsExpanded;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}