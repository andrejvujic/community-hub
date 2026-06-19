using System;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Resources.Localization
{
    public class LocalizedDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentCulture = TranslationSource.Instance.CurrentCulture ?? culture;

            if (value is double d)
            {
                return d.ToString("F2", currentCulture);
            }

            if (value is string str)
            {
                if (str == "/" || string.IsNullOrWhiteSpace(str))
                    return "/";

                if (double.TryParse(str, CultureInfo.InvariantCulture, out double parsedDouble))
                {
                    return parsedDouble.ToString("F2", currentCulture);
                }
            }

            if (value is float f)
            {
                return f.ToString("F2", currentCulture);
            }

            return "/";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}