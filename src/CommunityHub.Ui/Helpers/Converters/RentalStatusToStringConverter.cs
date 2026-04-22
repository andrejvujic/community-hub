using CommunityHub.Application.Domain.CommonRoomRentalRequest;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class RentalStatusToStringConverter : IValueConverter
    {
        private readonly string _approvedText = "Odobren";
        private readonly string _rejectedText = "Odbijen";
        private readonly string _waitingText = "Na čekanju";
        private readonly string _waitingDateChangeText = "Na čekanju - promena termina";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not RentalStatus status) return null;

            switch (status)
            {
                case RentalStatus.Approved:
                    return _approvedText;
                case RentalStatus.Rejected:
                    return _rejectedText;
                case RentalStatus.Waiting:
                    return _waitingText;
                case RentalStatus.WaitingDateChange:
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
