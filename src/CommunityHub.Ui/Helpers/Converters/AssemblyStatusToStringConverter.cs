using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class AssemblyStatusToStringConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not AssemblyStatus status) return null;

            return status switch
            {
                AssemblyStatus.Scheduled => "Zakazana",
                AssemblyStatus.Confirmed => "Potvrđena",
                AssemblyStatus.Cancelled => "Otkazana",
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
