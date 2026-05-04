using CommunityHub.Application.Modules.Shared.Core.Domain.Notifications;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class NotificationStatusToIconCodeConverter : IValueConverter
    {
        private readonly string _readIconCode = "\uf2b6";
        private readonly string _unreadIconCode = "\uf0e0";
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not NotificationStatus status)
                return null;

            return status switch
            {
                NotificationStatus.Read => _readIconCode,
                NotificationStatus.Unread => _unreadIconCode,
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
