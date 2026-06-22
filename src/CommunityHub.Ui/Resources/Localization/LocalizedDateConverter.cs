using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Resources.Localization
{
    public class LocalizedDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt) return TranslationSource.Instance.FormatDate(dt);
            if (value is DateOnly d) return TranslationSource.Instance.FormatDate(d);
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
