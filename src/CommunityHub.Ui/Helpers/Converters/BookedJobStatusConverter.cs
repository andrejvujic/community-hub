using System.Globalization;
using System.Windows.Data;
using CommunityHub.Ui.Resources.Localization;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class BookedJobStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string status) return string.Empty;
            return TranslationSource.Instance[$"BookedJobStatus_{status}"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
