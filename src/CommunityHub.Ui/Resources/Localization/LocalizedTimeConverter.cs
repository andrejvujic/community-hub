using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Resources.Localization
{
    public class LocalizedTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? dateTimeToFormat = null;

            if (value is DateTime dt)
            {
                dateTimeToFormat = dt;
            }
            else if (value is TimeOnly time)
            {
                dateTimeToFormat = DateTime.Today.Add(time.ToTimeSpan());
            }

            if (dateTimeToFormat.HasValue)
            {
                var currentCulture = TranslationSource.Instance.CurrentCulture ?? culture;

                if (currentCulture.Name.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                {
                    return dateTimeToFormat.Value.ToString("h:mm tt", currentCulture);
                }

                return dateTimeToFormat.Value.ToString("HH:mm", currentCulture);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}