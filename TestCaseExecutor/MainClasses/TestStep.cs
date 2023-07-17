﻿using Newtonsoft.Json;
using System;
using System.Windows.Input;
using System.Windows.Media;
using TestCaseExecutor.Commands;
using TestCaseExecutor.Common;

namespace TestCaseExecutor.MainClasses
{
    /// <summary>
    /// Class for each test step in a testcase
    /// </summary>
    internal class TestStep : Notify
    {
        internal TestStep()
        {
            BtnTestStepSuccess = new RelayCommand<object>(BtnTestStepSuccessExecute);
            BtnTestStepFailed = new RelayCommand<object>(BtnTestStepFailedExecute);
        }

        public string? TestStepID { get; set; }
        public string? StepAction { get; set; }
        public string? StepExpected { get; set; }
        public string? AdditionalUserText { get; set; }
        public bool TestStepSuccess { get; set; } = false;
        public bool TestStepExecuted { get; set; } = false; 

        // ignore buttons for JSON export
        [JsonIgnore]
        public ICommand BtnTestStepFailed { get; private set; }
        [JsonIgnore]
        public ICommand BtnTestStepSuccess { get; private set; }

        internal event EventHandler? TestStepStatusChanged;

        protected virtual void OnTestStepStatusChanged()
        {
            TestStepStatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private Brush _btnSuccessColor = Brushes.Gray;
        [JsonIgnore]
        public Brush BtnSuccessColor
        {
            get => _btnSuccessColor;
            set
            {
                _btnSuccessColor = value;
                OnPropertyChanged();
            }
        }

        private Brush _btnFailedColor = Brushes.Gray;
        [JsonIgnore]
        public Brush BtnFailedColor
        {
            get => _btnFailedColor;
            set
            {
                _btnFailedColor = value;
                OnPropertyChanged();
            }
        }

        private void BtnTestStepSuccessExecute(object? obj)
        {
            BtnSuccessColor = Brushes.Green;
            BtnFailedColor = Brushes.Gray;
            TestStepSuccess = true;
            TestStepExecuted = true;
            OnTestStepStatusChanged();
        }

        private void BtnTestStepFailedExecute(object? obj)
        {
            BtnFailedColor = Brushes.Red;
            BtnSuccessColor = Brushes.Gray;
            TestStepSuccess = false;
            TestStepExecuted = true;
            OnTestStepStatusChanged();
        }

        /// <summary>
        /// updatemethod for the state if the user loads the saved JSON file
        /// only run the executon method if the step was executed previously
        /// </summary>        
        internal void UpdateTestStepState()
        {            
            if (TestStepExecuted && TestStepSuccess)
            {
                BtnTestStepSuccessExecute(null);
            }
            else if (TestStepExecuted)
            {
                BtnTestStepFailedExecute(null);
            }
        }
    }
}
