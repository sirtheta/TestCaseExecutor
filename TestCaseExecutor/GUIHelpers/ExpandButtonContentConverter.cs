using System;
using System.Globalization;
using System.Windows.Data;

namespace TestCaseExecutor.Helpers
{
    internal class ExpandButtonContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isExpanded = (bool)value;
            return isExpanded ? "-" : "+";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
