using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class IEnumerableContainsToBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) return false;

            var collection = values[0] as IEnumerable;
            if (collection is null) return false;

            var targetElement = values[1];

            foreach (var element in collection)
                if (Equals(element, targetElement)) return true;

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
