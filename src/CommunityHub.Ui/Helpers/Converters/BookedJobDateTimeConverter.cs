using System.Globalization;
using System.Windows.Data;
using CommunityHub.Ui.Resources.Localization;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class BookedJobDateTimeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not DateOnly date) return string.Empty;

            var dateStr = TranslationSource.Instance.FormatDate(date);

            if (values[1] is TimeOnly time)
                return $"{dateStr} {TranslationSource.Instance["Service_At"]} {TranslationSource.Instance.FormatTime(time)}";

            return dateStr;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
