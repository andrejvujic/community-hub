using CommunityHub.Application.Domain.Neighborhoods;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class NeighborhoodAccessRequestStatusToStringConverter : IValueConverter
    {
        private const string _waitingText = "Čeka odobrenje";
        private const string _acceptedText = "Prihvaćen";
        private const string _rejectedText = "Odbijen";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not NeighborhoodAccessRequestStatus status)
                return null;

            switch (status)
            {
                case NeighborhoodAccessRequestStatus.Waiting:
                    return _waitingText;
                case NeighborhoodAccessRequestStatus.Accepted:
                    return _acceptedText;
                case NeighborhoodAccessRequestStatus.Rejected:
                    return _rejectedText;
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string statusText)
                return null;

            if (string.IsNullOrWhiteSpace(statusText))
                return null;

            switch (statusText)
            {
                case _waitingText:
                    return NeighborhoodAccessRequestStatus.Waiting;
                case _acceptedText:
                    return NeighborhoodAccessRequestStatus.Accepted;
                case _rejectedText:
                    return NeighborhoodAccessRequestStatus.Rejected;
            }

            return null;
        }
    }
}