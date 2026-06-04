using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class AssemblyStatusToIconCodeConverter : IValueConverter
    {
        private const string _scheduledIconCode = "\uf017";
        private const string _confirmedIconCode = "\uf058";
        private const string _cancelledIconCode = "\uf057";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not AssemblyStatus status) return null;

            return status switch
            {
                AssemblyStatus.Scheduled => _scheduledIconCode,
                AssemblyStatus.Confirmed => _confirmedIconCode,
                AssemblyStatus.Cancelled => _cancelledIconCode,
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
