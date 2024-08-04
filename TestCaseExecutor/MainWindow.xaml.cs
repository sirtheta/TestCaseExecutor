/*
 * Copyright (C) 2024 Michael Neuhaus
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

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
