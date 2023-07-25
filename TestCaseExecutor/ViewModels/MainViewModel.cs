using MaterialDesignMessageBoxSirTheta;
using MaterialDesignMessageBoxSirTheta.Definitions;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TestCaseExecutor.Commands;
using TestCaseExecutor.Logic;
using TestCaseExecutor.MainClasses;

namespace TestCaseExecutor.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            BtnLoadCSVFile = new RelayCommand<object>(LoadCSVFile);
            BtnSaveCurrentTestSuite = new RelayCommand<object>(SaveCurrentTestSuite);
            BtnLoadSavedTestSuite = new RelayCommand<object>(LoadSavedTestSuite);
        }

        private ObservableCollection<TestCase> _testCaseCollection = new();
        private System.Threading.Timer? autosaveTimer;

        public ObservableCollection<TestCase> TestCaseCollection
        {
            get => _testCaseCollection;
            set
            {
                _testCaseCollection = value;
                OnPropertyChanged();
            }
        }

        private string? FileExportPath { get; set; } = null;

        public ICommand BtnLoadCSVFile { get; private set; }
        public ICommand BtnSaveCurrentTestSuite { get; private set; }
        public ICommand BtnLoadSavedTestSuite { get; private set; }

        private readonly int _timerInterval = 10000;

        /// <summary>
        /// Initializing the timer for autosave
        /// </summary>
        private void InitializeTimer()
        {
            autosaveTimer = new System.Threading.Timer(AutosaveCallback, null, _timerInterval, System.Threading.Timeout.Infinite);
        }

        /// <summary>
        /// Callback for the autosave method. 
        /// </summary>
        /// <param name="state"></param>
        private void AutosaveCallback(object? state)
        {
            bool anyChangesDetected = false;

            // Iterate through test cases and test steps to check for changes
            foreach (var testCase in TestCaseCollection)
            {
                foreach (var testStep in testCase.TestSteps)
                {
                    if (FileExportPath != null && testStep.AdditionalUserTextChanged)
                    {
                        anyChangesDetected = true;
                        testStep.AdditionalUserTextChanged = false;
                    }
                }
            }

            // Save the data if any changes were detected
            if (anyChangesDetected)
            {
                SaveCurrentTestSuite(null); // Save all the necessary data
            }

            // Restart the timer to run again after the specified interval
            autosaveTimer?.Change(_timerInterval, System.Threading.Timeout.Infinite);
        }

        // Import the testsuite from a csv file
        private void LoadCSVFile(object obj)
        {

            // set to null, needs new confirmation for saving
            FileExportPath = null;
            OpenFileDialog ofd = new()
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Open CSV file"
            };

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    if (TestCaseCollection.Count > 0 &&
                        MaterialDesignMessageBox.Show("Test suite aktualisieren oder eine neue Laden?", MessageType.Question, MessageButtons.Custom, "Aktualisieren", "neue Suite laden") == MaterialDesignMessageBoxResult.Yes)
                    {
                        LoadCSVFileToList.UpdateTestCasesFromCSV(ofd.FileName, TestCaseCollection);
                    }
                    else
                    {
                        TestCaseCollection = new ObservableCollection<TestCase>(LoadCSVFileToList.LoadCSVFile(ofd.FileName));

                    }
                    ShowNotification("Erfolg", "Test suite erfolgreich geladen.", NotificationType.Success);
                }
                catch (System.Exception)
                {
                    ShowNotification("Error", "Datei konnte nicht geladen werden", NotificationType.Error);
                }
            }
        }

        // save current state of testsuite to JSON
        private void SaveCurrentTestSuite(object? obj)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "JSON files (*.json)|*.json"
            };

            // if the file is not saved until now, store the file path for later
            if (FileExportPath == null && saveFileDialog.ShowDialog() == true)
            {
                var fileName = saveFileDialog.FileName;
                FileExportPath = fileName;
                InitializeTimer();
            }

            if (FileExportPath != null)
            {
                SaveAndLoadTestData.SaveTestDataFile(FileExportPath, TestCaseCollection);
                ShowNotification("Erfolg", "Test Suite gespeichert.", NotificationType.Success);
            }
            else
            {
                ShowNotification("Error", "Error beim speichern der Test suite.", NotificationType.Error);
            }
        }

        // Load saved test suite from JSON
        private void LoadSavedTestSuite(object? obj)
        {
            // set to null, needs new confirmation for saving
            FileExportPath = null;

            OpenFileDialog ofd = new()
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Open JSON file"
            };

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    TestCaseCollection = new ObservableCollection<TestCase>(SaveAndLoadTestData.LoadTestDataFile(ofd.FileName));
                    foreach (var testCase in TestCaseCollection)
                    {
                        testCase.UpdateStates();
                    }
                    ShowNotification("Erfolg", "Test suite erfolgreich geladen.", NotificationType.Success);
                }
                catch (System.Exception)
                {
                    ShowNotification("Error", "Datei konnte nicht geladen werden", NotificationType.Error);
                }

            }
        }
    }
}
