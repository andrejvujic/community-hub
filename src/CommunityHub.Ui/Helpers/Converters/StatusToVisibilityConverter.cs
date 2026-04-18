using CommunityHub.Application.Domain;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Status status) return Visibility.Collapsed;
            return status == Status.Waiting ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
