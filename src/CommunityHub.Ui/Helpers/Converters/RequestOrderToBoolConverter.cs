using CommunityHub.Application.Helpers.Sorting;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class RequestOrderToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not RequestOrder order || parameter is not string param)
                return false;

            return param == "Descending"
                ? order == RequestOrder.CreationDateDescending
                : order == RequestOrder.CreationDateAscending;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter is not string param || value is not true)
                return RequestOrder.CreationDateDescending;

            return param == "Descending"
                ? RequestOrder.CreationDateDescending
                : RequestOrder.CreationDateAscending;
        }
    }
}
