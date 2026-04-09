using CommunityHub.Application.Domain;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class EqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            City? c1 = (City?)values[0];
            City? c2 = (City?)values[1];
            Debug.WriteLine($"Comparing: {c1?.Name} and {c2?.Name} -> {Equals(c1, c2)}");
            return Equals(values[0], values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
