using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Helpers.Query;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    class RequestOrderToStringConverter : IValueConverter
    {
        private const string _unspecifiedText = "Bez sortiranja";
        private const string _creationDateAscendingText = "Rastuće";
        private const string _creationDateDescendingText = "Opadajuće";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not RequestOrder order)
                return null;

            switch (order)
            {
                case RequestOrder.Unspecified:
                    return _unspecifiedText;
                case RequestOrder.CreationDateAscending:
                    return _creationDateAscendingText;
                case RequestOrder.CreationDateDescending:
                    return _creationDateDescendingText;
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string orderText)
                return null;

            if (string.IsNullOrWhiteSpace(orderText))
                return null;

            switch (orderText)
            {
                case _unspecifiedText:
                    return RequestOrder.Unspecified;
                case _creationDateAscendingText:
                    return RequestOrder.CreationDateAscending;
                case _creationDateDescendingText:
                    return RequestOrder.CreationDateDescending;
            }

            return null;
        }
    }
}
