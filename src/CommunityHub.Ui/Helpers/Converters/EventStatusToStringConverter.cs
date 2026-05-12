using CommunityHub.Application.Domain.Events;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class EventStatusToStringConverter : IValueConverter
    {
        private const string PreparationText = "Priprema";
        private const string ScheduledText = "Zakazano";
        private const string FinishedText = "Završeno";
        private const string CancelledText = "Otkazano";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not EventStatus status)
                return null;

            return status switch
            {
                EventStatus.Preparation => PreparationText,
                EventStatus.Scheduled => ScheduledText,
                EventStatus.Finished => FinishedText,
                EventStatus.Cancelled => CancelledText,
                _ => null
            };
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string s)
                return null;

            if (string.IsNullOrWhiteSpace(s)) return null;

            return s switch
            {
                PreparationText => EventStatus.Preparation,
                ScheduledText => EventStatus.Scheduled,
                FinishedText => EventStatus.Finished,
                CancelledText => EventStatus.Cancelled,
                _ => null
            };
        }
    }
}
