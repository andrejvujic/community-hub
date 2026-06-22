using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Resources.Localization
{
    public class EnumLocalizationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            string enumKey = value.ToString();
            string localizedValue = TranslationSource.Instance[enumKey];
            return localizedValue ?? enumKey;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
