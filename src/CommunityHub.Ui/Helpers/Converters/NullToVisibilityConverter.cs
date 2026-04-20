using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNull = value == null;
            bool invert = (parameter as string)?.Equals("Inverse", StringComparison.OrdinalIgnoreCase) == true;

            bool visible = invert ? !isNull : isNull;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
