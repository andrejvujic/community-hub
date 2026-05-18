using System;
using System.Globalization;
using System.Windows.Data;
using CommunityHub.Application.Domain.TrustRecords;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class TrustStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TrustStatus status)
            {
                switch (status)
                {
                    case TrustStatus.New:
                        return "Novi član";
                    case TrustStatus.Inactive:
                        return "Neaktivan građanin";
                    case TrustStatus.Active:
                        return "Aktivan građanin";
                    case TrustStatus.Prominent:
                        return "Istaknut građanin";
                    case TrustStatus.Trustworthy:
                        return "Građanin od poverenja";
                }
            }
            return "Nepoznat status";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
