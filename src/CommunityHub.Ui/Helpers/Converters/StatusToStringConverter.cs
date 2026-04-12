using CommunityHub.Application.Domain;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class StatusToStringConverter: IValueConverter
    {
        private const string _waitingText = "Čeka odobrenje";
        private const string _acceptedText = "Prihvaćen";
        private const string _rejectedText = "Odbijen";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Status status)
                return null;

            switch (status)
            {
                case Status.Waiting:
                    return _waitingText;
                case Status.Accepted:
                    return _acceptedText;
                case Status.Rejected:
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
                    return Status.Waiting;
                case _acceptedText:
                    return Status.Accepted;
                case _rejectedText:
                    return Status.Rejected;
            }

            return null;
        }
    }
}