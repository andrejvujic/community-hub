using CommunityHub.Application.Domain;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class StatusToIconCodeConverter : IValueConverter
    {
        private const string _waitingIconCode = "\uf017";
        private const string _acceptedIconCode = "\uf058";
        private const string _rejectedIconCode = "\uf057";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Status status)
                return null;

            switch(status)
            {
                case Status.Waiting:
                    return _waitingIconCode;
                case Status.Accepted:
                    return _acceptedIconCode;
                case Status.Rejected:
                    return _rejectedIconCode;
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string iconCode)
                return null;

            if (string.IsNullOrWhiteSpace(iconCode))
                return null;

            switch (iconCode)
            {
                case _waitingIconCode:
                    return Status.Waiting;
                case _acceptedIconCode:
                    return Status.Accepted;
                case _rejectedIconCode:
                    return Status.Rejected;
            }

            return null;
        }
    }
}
