using System.Globalization;
using System.Windows.Data;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class UserToNameSurnameConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not UserDto user) return "N/A";

            return $"{user.Name} {user.Surname}";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
