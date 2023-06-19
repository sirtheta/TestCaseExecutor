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
            BtnSaveCSVFile = new RelayCommand<object>(SaveCSVFile);
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

        public ICommand BtnSaveCSVFile { get; private set; }

        public bool CheckBoxClicked
        {
            get => _checkBoxClicked;
            set
            {
                _checkBoxClicked = value;
                OnPropertyChanged();
            }
        }


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
                TestCaseCollection = new ObservableCollection<TestCase>(load.LoadCSVFile(ofd.FileName));
            }
        }

        private void SaveCSVFile(object obj)
        {
            SaveCSVFile saveCSVFile = new();
            SaveFileDialog saveFileDialog = new()
            {
                Filter = "CSV files (*.csv)|*.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var fileName = saveFileDialog.FileName;
                if (saveCSVFile.ExportCSVFile(fileName))
                {
                    ShowNotification("Success", "CSV saved successfully.", NotificationType.Success);
                }
            }
        }
    }
}
