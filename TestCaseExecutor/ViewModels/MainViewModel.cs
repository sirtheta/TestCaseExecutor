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
        private bool _checkBoxClicked;

        public ObservableCollection<TestCase> TestCaseCollection
        {
            get => _testCaseCollection;
            set
            {
                _testCaseCollection = value;
                OnPropertyChanged();
            }
        }

        public ICommand BtnLoadCSVFile { get; private set; }
        public ICommand BtnSaveCurrentTestSuite { get; private set; }
        public ICommand BtnLoadSavedTestSuite { get; private set; }

        // gets or sets the checkbox for each teststep
        public bool CheckBoxClicked
        {
            get => _checkBoxClicked;
            set
            {
                _checkBoxClicked = value;
                OnPropertyChanged();
            }
        }        

        // Import the testsuite from a csv file
        private void LoadCSVFile(object obj)
        {
            LoadCSVFileToList load = new();
            OpenFileDialog ofd = new()
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Open CSV file"
            };

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    TestCaseCollection = new ObservableCollection<TestCase>(load.LoadCSVFile(ofd.FileName));
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

            if (saveFileDialog.ShowDialog() == true)
            {
                var fileName = saveFileDialog.FileName;
                SaveAndLoadTestData.SaveTestDataFile(fileName, TestCaseCollection);

                ShowNotification("Success", "Current test suite saved successfully.", NotificationType.Success);
            }
        }

        // Load saved test suite from JSON
        private void LoadSavedTestSuite(object obj)
        {
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
