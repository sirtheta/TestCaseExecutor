using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using TestCaseExecutor.Commands;
using TestCaseExecutor.Common;

namespace TestCaseExecutor.MainClasses
{
    /// <summary>
    /// Class for the testcases
    /// </summary>
    internal class TestCase : Notify
    {
        public string? ID { get; set; }
        public string? Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        public bool AllStepsSuccessfully { get; set; } = false;
        public ObservableCollection<TestStep> TestSteps { get; set; } = new();

        private bool _isExpanded = false;
        private string? _title = null;

        public TestCase()
        {
            ToggleExpandCommand = new RelayCommand<object>(ToggleExpand);
            // add observer for the collection of the test steps to get notified if a test step is executed
            ((INotifyCollectionChanged)TestSteps).CollectionChanged += TestStepsCollectionChanged;
        }

        // handles the notification of the test steps
        private void TestStepsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newTestStep in e.NewItems?.OfType<TestStep>() ?? Enumerable.Empty<TestStep>())
                {
                    newTestStep.TestStepStatusChanged += TestStepSuccessOrFailedButtonClick;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldTestStep in e.OldItems?.OfType<TestStep>() ?? Enumerable.Empty<TestStep>())
                {
                    oldTestStep.TestStepStatusChanged -= TestStepSuccessOrFailedButtonClick;
                }
            }
        }

        // 
        private void TestStepSuccessOrFailedButtonClick(object? sender, EventArgs e)
        {
            if (TestSteps.All(step => step.TestStepSuccess))
            {
                ChangeIconAndColorOfTestStepSuccessState(true);
                AllStepsSuccessfully = true;
            }
            else
            {
                ChangeIconAndColorOfTestStepSuccessState(false);
                AllStepsSuccessfully = false;
            }
        }

        [JsonIgnore]
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        // command to toggle the expand of the testcase
        [JsonIgnore]
        public ICommand ToggleExpandCommand { get; }

        private void ToggleExpand(object obj)
        {
            IsExpanded = !IsExpanded;
        }

        private Brush _allTestStepSuccessStateColor = Brushes.Gray;
        [JsonIgnore]
        public Brush AllTestStepSuccessStateColor
        {
            get => _allTestStepSuccessStateColor;
            set
            {
                _allTestStepSuccessStateColor = value;
                OnPropertyChanged();
            }
        }

        private PackIconKind _allTestStepSuccessStateIcon = PackIconKind.Arrow;
        [JsonIgnore]
        public PackIconKind AllTestStepSuccessStateIcon
        {
            get => _allTestStepSuccessStateIcon;
            set
            {
                _allTestStepSuccessStateIcon = value;
                OnPropertyChanged();
            }
        }

        private void ChangeIconAndColorOfTestStepSuccessState(bool success)
        {
            AllTestStepSuccessStateColor = success ? Brushes.Green : Brushes.Red;
            AllTestStepSuccessStateIcon = success ? PackIconKind.Check : PackIconKind.Close;
        }

        internal void UpdateStates()
        {
            foreach (var item in TestSteps)
            {
                item.UpdateTestStepState();
            }
        }
    }
}

