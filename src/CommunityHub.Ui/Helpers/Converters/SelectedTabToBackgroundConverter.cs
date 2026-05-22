using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class SelectedTabToBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string selectedTab || parameter is not string tabName)
                return Binding.DoNothing;

            return selectedTab == tabName;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectedTabToBrushConverter : IMultiValueConverter
    {
        public object? Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Length < 3 || parameter is not string tabName)
                return Binding.DoNothing;

            var selectedTab = values[0] as string;
            var activeColor = values[1] as SolidColorBrush;
            var inactiveColor = values[2] as SolidColorBrush;

            if (selectedTab == tabName)
                return activeColor;
            else
                return inactiveColor;
        }

        public object?[] ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
