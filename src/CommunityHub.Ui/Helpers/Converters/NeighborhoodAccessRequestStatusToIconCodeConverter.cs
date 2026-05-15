using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Neighborhoods;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class NeighborhoodAccessRequestStatusToIconCodeConverter : IValueConverter
    {
        private const string _waitingIconCode = "\uf017";
        private const string _acceptedIconCode = "\uf058";
        private const string _rejectedIconCode = "\uf057";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not NeighborhoodAccessRequestStatus status)
                return null;

            switch (status)
            {
                case NeighborhoodAccessRequestStatus.Waiting:
                    return _waitingIconCode;
                case NeighborhoodAccessRequestStatus.Accepted:
                    return _acceptedIconCode;
                case NeighborhoodAccessRequestStatus.Rejected:
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
                    return NeighborhoodAccessRequestStatus.Waiting;
                case _acceptedIconCode:
                    return NeighborhoodAccessRequestStatus.Accepted;
                case _rejectedIconCode:
                    return NeighborhoodAccessRequestStatus.Rejected;
            }

            return null;
        }
    }
}
