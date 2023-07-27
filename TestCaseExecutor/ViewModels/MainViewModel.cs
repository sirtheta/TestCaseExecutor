using MaterialDesignMessageBoxSirTheta;
using MaterialDesignMessageBoxSirTheta.Definitions;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections.ObjectModel;
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
            BtnGenerateTestReport = new RelayCommand<object>(GenerateTestReport);
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

        public TestSuite TestSuite
        {
            get => _testSuite;
            set
            {
                _testSuite = value;
                OnPropertyChanged();
            }
        }

        private TestSuite _testSuite = new();

        private string? FileExportPath { get; set; } = null;

        public ICommand BtnLoadCSVFile { get; private set; }
        public ICommand BtnSaveCurrentTestSuite { get; private set; }
        public ICommand BtnLoadSavedTestSuite { get; private set; }
        public ICommand BtnGenerateTestReport { get; private set; }

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

        /// <summary>
        /// Import the testsuite from a csv file
        /// </summary>
        /// <param name="obj"></param>
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
                        LoadCSVFileToList.UpdateTestCasesFromCSV(ofd.FileName, TestSuite);
                    }
                    else
                    {
                        TestSuite = LoadCSVFileToList.LoadCSVFile(ofd.FileName);
                        TestCaseCollection = TestSuite.TestCases;
                    }
                    ShowNotification("Erfolg", "Test suite erfolgreich geladen.", NotificationType.Success);
                }
                catch (System.Exception)
                {
                    ShowNotification("Error", "Datei konnte nicht geladen werden", NotificationType.Error);
                }
            }
        }

        /// <summary>
        /// save current state of testsuite to JSON
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCurrentTestSuite(object? obj)
        {
            if (TestCaseCollection.Count > 0)
            {
                SaveFileDialog saveFileDialog = new()
                {
                    Filter = "JSON files (*.json)|*.json",
                    FileName = TestSuite.TestSuiteName
                };

                try
                {
                    // if the file is not saved until now, store the file path for later
                    if (FileExportPath == null && saveFileDialog.ShowDialog() == true)
                    {
                        var fileName = saveFileDialog.FileName;
                        FileExportPath = fileName;
                        InitializeTimer();
                    }

                    ArgumentNullException.ThrowIfNull(FileExportPath);
                    SaveAndLoadTestData.SaveTestDataFile(FileExportPath, TestSuite);
                    ShowNotification("Erfolg", "Test Suite gespeichert.", NotificationType.Success);
                }
                catch (System.Exception)
                {
                    ShowNotification("Error", "Fehler beim speichern der Test suite.", NotificationType.Error);
                }
            }
            else
            {
                NoDataWarning();
            }
        }

        /// <summary>
        /// Load saved test suite from JSON
        /// </summary>
        /// <param name="obj"></param>
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
                    TestSuite = SaveAndLoadTestData.LoadTestDataFile(ofd.FileName);
                    TestCaseCollection = TestSuite.TestCases;
                    foreach (var testCase in TestCaseCollection)
                    {
                        testCase.UpdateStates();
                    }
                    ShowNotification("Erfolg", "Test suite erfolgreich geladen.", NotificationType.Success);
                }
                catch (System.Exception)
                {
                    ShowNotification("Error", "Fehler beim laden der Datei.", NotificationType.Error);
                }

            }
        }

        /// <summary>
        /// generates a PDF report of the testSuite with failed or success state for each case and step
        /// </summary>
        /// <param name="obj"></param>
        private void GenerateTestReport(object? obj)
        {
            if (TestCaseCollection.Count > 0)
            {
                try
                {
                    SaveFileDialog saveFileDialog = new()
                    {
                        Filter = "PDF files (*.pdf)|*.pdf",
                        FileName = TestSuite.TestSuiteName
                    };

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        GeneratePDFReport.Build(TestSuite).Build(saveFileDialog.FileName);
                        ShowNotification("Erfolg", "PDF erfolgreich erstellt.", NotificationType.Success);
                    }
                }
                catch (System.Exception)
                {
                    ShowNotification("Error", "Fehler beim speichern des PDF.", NotificationType.Error);
                }
            }
            else
            {
                NoDataWarning();
            }
        }

        private static void NoDataWarning()
        {
            ShowNotification("Warnung", "Keine Daten zum Speichern vorhanden!.", NotificationType.Warning);
        }
    }
}
