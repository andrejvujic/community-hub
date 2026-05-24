using System.Globalization;
using System.Windows.Data;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class MeetingStatusToIconCodeConverter : IValueConverter
    {
        private const string _inPreparationIconCode = "\uf017";
        private const string _scheduledIconCode = "\uf058";
        private const string _cancelledIconCode = "\uf057";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not MeetingStatus status)
                return null;

            return status switch
            {
                MeetingStatus.InPreparation => _inPreparationIconCode,
                MeetingStatus.Scheduled => _scheduledIconCode,
                MeetingStatus.Cancelled => _cancelledIconCode,
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
