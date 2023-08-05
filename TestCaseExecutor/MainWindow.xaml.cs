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
        }

        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            mainViewModel.MainWindowWidth = e.NewSize.Width;
        }
    }
}
