using Microsoft.Win32;
using Notifications.Wpf.Core;
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
        }

        private ObservableCollection<TestCase> _testCaseCollection = new();

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
                    TestCaseCollection = new ObservableCollection<TestCase>(LoadCSVFileToList.LoadCSVFile(ofd.FileName));
                    ShowNotification("Success", "Test suite successfully imported.", NotificationType.Success);
                }
                catch (System.Exception)
                {
                    ShowNotification("Error", "File could not be loaded", NotificationType.Error);
                }
            }
        }

        // save current state of testsuite to JSON
        private void SaveCurrentTestSuite(object obj)
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
            }

            if (FileExportPath != null)
            {
                SaveAndLoadTestData.SaveTestDataFile(FileExportPath, TestCaseCollection);
                ShowNotification("Success", "Current test suite saved successfully.", NotificationType.Success);
            }
            else
            {
                ShowNotification("Error", "Error saving test suite.", NotificationType.Error);
            }
        }

        // Load saved test suite from JSON
        private void LoadSavedTestSuite(object obj)
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
                    ShowNotification("Success", "Test suite successfully loaded.", NotificationType.Success);
                }
                catch (System.Exception)
                {
                    ShowNotification("Error", "File could not be loaded", NotificationType.Error);
                }

            }
        }
    }
}
