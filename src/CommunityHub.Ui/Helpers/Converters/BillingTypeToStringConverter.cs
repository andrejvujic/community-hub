using CommunityHub.Application.Domain.Services;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class BillingTypeToStringConverter : IValueConverter
    {
        private const string _perHourText = "Plaćanje po satu";
        private const string _perJobText = "Plaćanje po usluzi";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not BillingType type) 
                return null;

            switch (type)
            {
                case BillingType.PerHour:
                    return _perHourText;
                case BillingType.PerJob:
                    return _perJobText;
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string billingText)
                return null;

            if (string.IsNullOrWhiteSpace(billingText))
                return null;

            switch (billingText)
            {
                case _perHourText:
                    return BillingType.PerHour;
                case _perJobText:
                    return BillingType.PerJob;
            }

            return null;
        }
    }
}