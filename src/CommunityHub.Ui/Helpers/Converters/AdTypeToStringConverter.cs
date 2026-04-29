using CommunityHub.Application.Domain.Buildings.Ads;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class AdTypeToStringConverter : IValueConverter
    {
        private readonly string _offerText = "Nudim";
        private readonly string _requestText = "Tražim";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? defaultText = parameter is string parameterValue ? parameterValue : null;

            if (value is not AdType type) return defaultText;

            switch (type)
            {
                case AdType.Offer:
                    return _offerText;
                case AdType.Request:
                    return _requestText;
            }

            return defaultText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
