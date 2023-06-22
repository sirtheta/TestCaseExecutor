using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestCaseExecutor.Common
{
    /// <summary>
    /// Implementation of INotifyPropertyChanged
    /// </summary>
    internal class Notify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
