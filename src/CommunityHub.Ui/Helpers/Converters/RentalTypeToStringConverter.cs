using CommunityHub.Application.Domain.CommonRoom;
using CommunityHub.Application.Domain.NoticeBoard;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class RentalTypeToStringConverter : IValueConverter
    {
        private readonly string _oneDayText = "Jednodnevno";
        private readonly string _multiDayText = "Višednevno";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not RentalType type) return null;

            switch (type)
            {
                case RentalType.OneDay:
                    return _oneDayText;
                case RentalType.MultiDay:
                    return _multiDayText;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
