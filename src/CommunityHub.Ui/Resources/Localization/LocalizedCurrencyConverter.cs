using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Resources.Localization
{
    public class LocalizedCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentCulture = TranslationSource.Instance.CurrentCulture ?? culture;

            if (value is decimal amount)
            {
                return $"{amount.ToString("N2", currentCulture)} RSD";
            }

            return $"{0m.ToString("N2", currentCulture)} RSD";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
