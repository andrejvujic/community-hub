using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class AssemblyTopicStatusToIconCodeConverter : IValueConverter
    {
        private const string _suggestedIconCode = "\uf017";
        private const string _acceptedIconCode = "\uf058";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not AssemblyTopicStatus status) return null;

            return status switch
            {
                AssemblyTopicStatus.Suggested => _suggestedIconCode,
                AssemblyTopicStatus.Accepted => _acceptedIconCode,
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
