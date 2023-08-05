using Newtonsoft.Json;
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
        public string? StepAction
        {
            get => _stepAction;
            set
            {
                _stepAction = value;
                OnPropertyChanged();
            }
        }
        public string? StepExpected
        {
            get => _stepExpected;
            set
            {
                _stepExpected = value;
                OnPropertyChanged();
            }
        }
        public string? AdditionalUserText
        {
            get => _additionalUserText;
            set
            {
                _additionalUserText = value;
                AdditionalUserTextChanged = true;
                OnPropertyChanged();
            }
        }
        public bool? TestStepSuccess { get; set; } = null;
        public bool TestStepExecuted { get; set; } = false;

        private string? _additionalUserText;
        private string? _stepAction = null;
        private string? _stepExpected = null;

        // ignore fields for JSON export
        [JsonIgnore]
        public ICommand BtnTestStepFailed { get; private set; }

        [JsonIgnore]
        public ICommand BtnTestStepSuccess { get; private set; }

        // used to detect if a user has changed some text. 
        [JsonIgnore]
        internal bool AdditionalUserTextChanged { get; set; } = false;

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

        private int _widthCol1;
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

        private int _widthCol3;
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

        private int _widthCol5;
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
            if (TestStepExecuted && TestStepSuccess == true)
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
