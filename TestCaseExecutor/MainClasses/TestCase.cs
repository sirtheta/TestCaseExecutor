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
        public bool? AllStepsSuccessfully { get; set; } = null;
        public ObservableCollection<TestStep> TestSteps { get; set; } = new();

        private bool _isExpanded = false;
        private string? _title = null;
        private bool _testStepStatusChanged = false;
        public TestCase()
        {
            ToggleExpandCommand = new RelayCommand<object>(ToggleExpand);
            // add observer for the collection of the test steps to get notified if a test step is executed
            ((INotifyCollectionChanged)TestSteps).CollectionChanged += TestStepsCollectionChanged;
        }

        /// <summary>
        /// handles the notification of the test steps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// handles the event of the success or failed button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TestStepSuccessOrFailedButtonClick(object? sender, EventArgs e)
        {
            if (TestSteps.All(step => step.TestStepSuccess == true))
            {
                ChangeIconAndColorOfTestStepSuccessState(true);
                AllStepsSuccessfully = true;
                TestStepStatusChanged = true;
            }
            else if (TestSteps.Any(step => step.TestStepSuccess == false))
            {
                ChangeIconAndColorOfTestStepSuccessState(false);
                AllStepsSuccessfully = false;
                TestStepStatusChanged = true;
            }
        }

        [JsonIgnore]
        public bool TestStepStatusChanged
        {
            get => _testStepStatusChanged;
            set => _testStepStatusChanged = value;
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
            AdaptWidthInTestSteps();
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

        private static readonly int _initialWidthColumn1 = 350;
        private static readonly int _initialWidthColumn3 = 300;
        private static readonly int _initialWidthColumn5 = 250;

        private int _widthCol1 = _initialWidthColumn1;
        [JsonIgnore]
        public int WidthCol1
        {
            get => _widthCol1;
            set
            {
                _widthCol1 = value;
                OnPropertyChanged();
            }
        }

        private int _widthCol3 = _initialWidthColumn3;
        [JsonIgnore]
        public int WidthCol3
        {
            get => _widthCol3;
            set
            {
                _widthCol3 = value;
                OnPropertyChanged();
            }
        }

        private int _widthCol5 = _initialWidthColumn5;
        [JsonIgnore]
        public int WidthCol5
        {
            get => _widthCol5;
            set
            {
                _widthCol5 = value;
                OnPropertyChanged();
            }
        }

        private void AdaptWidthInTestSteps()
        {
            foreach (var step in TestSteps)
            {
                step.WidthCol1 = WidthCol1;
                step.WidthCol3 = WidthCol3;
                step.WidthCol5 = WidthCol5;
            }
        }

        internal void AdaptWidthOnChange(double change)
        {
            WidthCol1 = (int)(_initialWidthColumn1 * change);
            WidthCol3 = (int)(_initialWidthColumn3 * change);
            WidthCol5 = (int)(_initialWidthColumn5 * change);
            AdaptWidthInTestSteps();
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

