using MaterialDesignMessageBoxSirTheta;
using MaterialDesignMessageBoxSirTheta.Definitions;
using Microsoft.Win32;
using Notifications.Wpf.Core;
using System;
using System.Collections.ObjectModel;
using System.IO;
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
            BtnLoadSavedTestSuite = new RelayCommand<object>(LoadSavedTestSuite);
            BtnSaveCurrentTestSuite = new RelayCommand<object>(SaveCurrentTestSuite);
            BtnSaveAsNew = new RelayCommand<object>(SaveAsNewFile);
            BtnGenerateTestReport = new RelayCommand<object>(GenerateTestReport);
        }

        private ObservableCollection<TestCase> _testCaseCollection = new();
        private System.Threading.Timer? _autosaveTimer;

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
        public ICommand BtnLoadSavedTestSuite { get; private set; }
        public ICommand BtnSaveCurrentTestSuite { get; private set; }
        public ICommand BtnSaveAsNew { get; private set; }
        public ICommand BtnGenerateTestReport { get; private set; }

        private static readonly int _timerInterval = 10000;
        private static readonly double _initialWidth = 1100;

        private double _mainWindowWidth;
        private string _copyright = $"Copyright © {DateTime.Now.Year} nemicomp. All rights reserved. Developed and designed by Michael Neuhaus, licensed under the MIT license. Version: {GetVersion()}";
        public string Copyright { get => _copyright; set => _copyright = value; }
        public double MainWindowWidth
        {
            get => _mainWindowWidth;
            set
            {
                _mainWindowWidth = value;
                AdaptWidthInTestCaseOnChange();
            }
        }

        private static string? GetVersion()
        {
            return System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString();
        }

        /// <summary>
        /// Check for changes in testcase and teststeps
        /// </summary>
        /// <returns>bool</returns>
        internal bool CheckForChanges()
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
                    }
                }
                if (testCase.TestStepStatusChanged)
                {
                    anyChangesDetected = true;
                }
            }

            return anyChangesDetected;
        }

        /// <summary>
        /// ask user with a dialog to save the current testsuite
        /// </summary>
        internal void AskForSaving()
        {
            if (MaterialDesignMessageBox.Show("Es sind Änderungen vorhanden. Möchtest Du speichern?", MessageType.Question, MessageButtons.Custom, "Speichern", "Verwerfen") == MaterialDesignMessageBoxResult.Yes)
            {
                SaveTestSuite();
            }
        }

        /// <summary>
        /// Reset the change state of all test cases and test steps
        /// </summary>
        private void ResetChangeState()
        {
            foreach (var testCase in TestCaseCollection)
            {
                foreach (TestStep testStep in testCase.TestSteps)
                {
                    testStep.AdditionalUserTextChanged = false;
                }

                testCase.TestStepStatusChanged = false;
            }
        }

        /// <summary>
        /// Adapts the width of the textboxes for all testcases. 
        /// (in the test steps as well)
        /// </summary>
        private void AdaptWidthInTestCaseOnChange()
        {
            double sizeChange = MainWindowWidth / _initialWidth;
            foreach (var testCase in TestCaseCollection)
            {
                testCase.AdaptWidthOnChange(sizeChange);
            }
        }

        /// <summary>
        /// Initializing the timer for autosave
        /// </summary>
        private void InitializeAutoSaveTimer()
        {
            _autosaveTimer = new System.Threading.Timer(AutosaveCallback, null, _timerInterval, System.Threading.Timeout.Infinite);
        }

        /// <summary>
        /// Callback for the autosave method. 
        /// </summary>
        /// <param name="state"></param>
        private void AutosaveCallback(object? state)
        {
            // Save the data if any changes were detected
            if (CheckForChanges())
            {
                SaveTestSuite();
            }

            // Restart the timer to run again after the specified interval
            _autosaveTimer?.Change(_timerInterval, System.Threading.Timeout.Infinite);
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
                        MaterialDesignMessageBox.Show("Möchtest Du die Test suite aktualisieren oder eine neue Laden?", MessageType.Question, MessageButtons.Custom, "Aktualisieren", "neue Suite laden") == MaterialDesignMessageBoxResult.Yes)
                    {
                        LoadCSVFileToList.UpdateTestCasesFromCSV(ofd.FileName, TestSuite);
                    }
                    else
                    {
                        TestSuite = LoadCSVFileToList.LoadCSVFile(ofd.FileName);
                        TestCaseCollection = TestSuite.TestCases;
                        ResetFileSaveSettings();
                    }
                    AdaptWidthInTestCaseOnChange();
                    ShowNotification("Erfolg", "Test suite erfolgreich geladen.", NotificationType.Success);
                }
                catch (Exception)
                {
                    ShowNotification("Error", "Datei konnte nicht geladen werden", NotificationType.Error);
                }
            }
        }

        /// <summary>
        /// Reset the settings for autosaving (filExportPath and timer)
        /// </summary>
        private void ResetFileSaveSettings()
        {
            FileExportPath = null;
            _autosaveTimer?.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

        }

        /// <summary>
        /// callback to save testsuite as new file
        /// </summary>
        /// <param name="obj"></param>
        private void SaveAsNewFile(object obj)
        {
            // store path if user abort saving process, restore it
            var tempPath = FileExportPath;
            ResetFileSaveSettings();
            if (!SaveTestSuite())
            {
                FileExportPath = tempPath;
                InitializeAutoSaveTimer();
            }
        }


        private void SaveCurrentTestSuite(object? obj)
        {
            SaveTestSuite();
        }

        /// <summary>
        /// Save current state of testsuite to JSON
        /// </summary>
        /// <param name="obj"></param>
        private bool SaveTestSuite()
        {
            bool retVal = false;
            if (TestCaseCollection.Count > 0)
            {
                SaveFileDialog saveFileDialog = new()
                {
                    Filter = "JSON files (*.json)|*.json",
                    FileName = TestSuite.TestSuiteName
                };

                try
                {
                    if (FileExportPath == null || !File.Exists(FileExportPath))
                    {
                        // Show the Save File dialog if FileExportPath is null or the file does not exist
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            FileExportPath = saveFileDialog.FileName;
                            InitializeAutoSaveTimer();
                        }
                        else
                        {
                            // Operation cancelled
                            return retVal;
                        }
                    }

                    if (!string.IsNullOrEmpty(FileExportPath))
                    {
                        SaveAndLoadTestData.SaveTestDataFile(FileExportPath, TestSuite);
                        // rest the change state after saving to json
                        ResetChangeState();
                        ShowNotification("Erfolg", "Test Suite gespeichert.", NotificationType.Success);
                        retVal = true;
                    }
                    else
                    {
                        ErrorSavingSuite();
                    }
                }
                catch (Exception)
                {
                    ErrorSavingSuite();
                }
            }
            else
            {
                NoDataWarning();
            }
            return retVal;
        }

        /// <summary>
        /// Load saved test suite from JSON
        /// </summary>
        /// <param name="obj"></param>
        private void LoadSavedTestSuite(object? obj)
        {
            // reset file saving settings
            ResetFileSaveSettings();

            OpenFileDialog openFileDialog = new()
            {
                Filter = "JSON files (*.json)|*.json",
                Title = "Open JSON file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    TestSuite = SaveAndLoadTestData.LoadTestDataFile(openFileDialog.FileName);
                    TestCaseCollection = TestSuite.TestCases;
                    foreach (var testCase in TestCaseCollection)
                    {
                        testCase.UpdateStates();
                    }
                    FileExportPath = openFileDialog.FileName;
                    AdaptWidthInTestCaseOnChange();
                    // initialize the autosave timer after loading from json
                    InitializeAutoSaveTimer();
                    // rest the change state after loading from json
                    ResetChangeState();
                    ShowNotification("Erfolg", "Test suite erfolgreich geladen.", NotificationType.Success);
                }
                catch (Exception)
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
                catch (Exception)
                {
                    ShowNotification("Error", "Fehler beim speichern des PDF.", NotificationType.Error);
                }
            }
            else
            {
                NoDataWarning();
            }
        }

        /// <summary>
        /// Shows a no data warning notification
        /// </summary>
        private static void NoDataWarning()
        {
            ShowNotification("Warnung", "Keine Daten zum Speichern vorhanden!.", NotificationType.Warning);
        }

        /// <summary>
        /// Shows a saving error in notification
        /// </summary>
        private static void ErrorSavingSuite()
        {
            ShowNotification("Error", "Fehler beim speichern der Test suite.", NotificationType.Error);
        }
    }
}
