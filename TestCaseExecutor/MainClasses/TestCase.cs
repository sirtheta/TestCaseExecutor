using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
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
        public string? Title { get; set; }
        public ObservableCollection<TestStep> TestSteps { get; set; } = new();

        private bool _isExpanded = false;
        private bool _allTestStepsExecuted = false;

        public TestCase()
        {
            ToggleExpandCommand = new RelayCommand<object>(ToggleExpand);
            ((INotifyCollectionChanged)TestSteps).CollectionChanged += TestStepsCollectionChanged;
        }

        private void TestStepsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var newTestStep in e.NewItems?.OfType<TestStep>() ?? Enumerable.Empty<TestStep>())
                {
                    newTestStep.CheckBoxClickedChanged += TestStepCheckBoxClickedChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldTestStep in e.OldItems?.OfType<TestStep>() ?? Enumerable.Empty<TestStep>())
                {
                    oldTestStep.CheckBoxClickedChanged -= TestStepCheckBoxClickedChanged;
                }
            }
        }

        private void TestStepCheckBoxClickedChanged(object? sender, EventArgs e)
        {
            if (AllCheckBoxesClicked)
            {
                AllTestStepsExecuted = true;
            }
            else
            {
                AllTestStepsExecuted = false;
            }
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
        public bool AllCheckBoxesClicked => TestSteps.All(step => step.CheckBoxClicked);

        // command to toggle the expand of the testcase
        public ICommand ToggleExpandCommand
        {
            get;
        }

        private void ToggleExpand(object obj)
        {
            IsExpanded = !IsExpanded;
        }
    }
}