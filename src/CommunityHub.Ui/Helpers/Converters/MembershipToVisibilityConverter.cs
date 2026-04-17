using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class MembershipToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasMembership = value != null;
            bool invert = (parameter as string)?.Equals("Inverse", StringComparison.OrdinalIgnoreCase) == true;

            bool result = invert ? !hasMembership : hasMembership;
            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
