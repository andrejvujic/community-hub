using CommunityHub.Application.Domain.Buildings.CommonRooms.RentalRequests;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class RentalRequestStatusToStringConverter : IValueConverter
    {
        private readonly string _approvedText = "Odobren";
        private readonly string _rejectedText = "Odbijen";
        private readonly string _waitingText = "Na čekanju";
        private readonly string _waitingDateChangeText = "Na čekanju — promena termina";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not RentalRequestStatus status) return null;

            switch (status)
            {
                case RentalRequestStatus.Approved:
                    return _approvedText;
                case RentalRequestStatus.Rejected:
                    return _rejectedText;
                case RentalRequestStatus.Waiting:
                    return _waitingText;
                case RentalRequestStatus.WaitingDateChange:
                    return _waitingDateChangeText;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
