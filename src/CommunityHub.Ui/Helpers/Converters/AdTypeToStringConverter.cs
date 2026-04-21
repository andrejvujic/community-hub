using CommunityHub.Application.Domain.NoticeBoard;
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
            if (value is not AdType type) return null;

            switch (type)
            {
                case AdType.Offer:
                    return _offerText;
                case AdType.Request:
                    return _requestText;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
