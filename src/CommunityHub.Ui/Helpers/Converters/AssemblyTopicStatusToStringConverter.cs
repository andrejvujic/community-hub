using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class AssemblyTopicStatusToStringConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not AssemblyTopicStatus status) return null;

            return status switch
            {
                AssemblyTopicStatus.Suggested => "Predložena",
                AssemblyTopicStatus.Accepted => "Prihvaćena",
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
