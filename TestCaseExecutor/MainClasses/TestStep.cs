namespace TestCaseExecutor.MainClasses
{
    internal class TestStep
    {
        /// <summary>
        /// Class for each test step in a testcase
        /// </summary>
        public string? TestStepID { get; set; }
        public string? StepAction { get; set; }
        public string? StepExpected { get; set; }
        public bool CheckBoxClicked { get; set; } = false;
    }
}
