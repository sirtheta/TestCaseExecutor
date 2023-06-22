using System;
using TestCaseExecutor.Common;

namespace TestCaseExecutor.MainClasses
{
    internal class TestStep : Notify
    {
        /// <summary>
        /// Class for each test step in a testcase
        /// </summary>
        public string? TestStepID { get; set; }
        public string? StepAction { get; set; }
        public string? StepExpected { get; set; }
       
        private bool _checkBoxClicked = false;

        public bool CheckBoxClicked
        {
            get => _checkBoxClicked;
            set
            {
                if (_checkBoxClicked != value)
                {
                    _checkBoxClicked = value;
                    OnCheckBoxClickedChanged();
                }
            }
        }

        public event EventHandler? CheckBoxClickedChanged;

        protected virtual void OnCheckBoxClickedChanged()
        {
            CheckBoxClickedChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
