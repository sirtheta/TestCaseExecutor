namespace TestCaseExecutor.MainClasses
{
    internal class TestStep
    {
        public string? TestStepID { get; set; }
        public string? StepAction { get; set; }
        public string? StepExpected { get; set; }
        public bool CheckBoxClicked { get; set; } = false;
    }
}
