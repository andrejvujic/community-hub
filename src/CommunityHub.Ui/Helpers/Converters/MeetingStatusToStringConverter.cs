using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class MeetingStatusToStringConverter : IValueConverter
    {
        private readonly string _inPreparationText = "U pripremi";
        private readonly string _scheduledText = "Zakazan";
        private readonly string _cancelledText = "Otkazan";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not MeetingStatus status) return null;

            switch (status)
            {
                case MeetingStatus.InPreparation:
                    return _inPreparationText;
                case MeetingStatus.Scheduled:
                    return _scheduledText;
                case MeetingStatus.Cancelled:
                    return _cancelledText;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
