using System.ComponentModel;
using System.Windows;
using TestCaseExecutor.ViewModels;

namespace TestCaseExecutor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainViewModel mainViewModel = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = mainViewModel;
            // add size changed event handler
            SizeChanged += OnWindowSizeChanged;
            Application.Current.MainWindow.Closing += new CancelEventHandler(MainWindow_Closing);
        }

        protected void OnWindowSizeChanged(object? sender, SizeChangedEventArgs e)
        {
            mainViewModel.MainWindowWidth = e.NewSize.Width;
        }       

        void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            if (mainViewModel.CheckForChanges())
            {
                mainViewModel.AskForSaving();
            }
        }
    }
}
