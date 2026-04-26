using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class RentalRequestStatusToIconCodeConverter : IValueConverter
    {
        private const string _waitingIconCode = "\uf017";
        private const string _waitingDateChangeIconCode = "\uf017";
        private const string _acceptedIconCode = "\uf058";
        private const string _rejectedIconCode = "\uf057";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not RentalRequestStatus status)
                return null;

            switch (status)
            {
                case RentalRequestStatus.Waiting:
                    return _waitingIconCode;
                case RentalRequestStatus.WaitingDateChange:
                    return _waitingDateChangeIconCode;
                case RentalRequestStatus.Approved:
                    return _acceptedIconCode;
                case RentalRequestStatus.Rejected:
                    return _rejectedIconCode;
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}