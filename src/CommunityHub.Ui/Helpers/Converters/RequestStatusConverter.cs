using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class RequestStatusConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString();

            if (status == "Waiting")
                return "Čeka odobrenje";

            if (status == "Accepted")
                return "Prihvaćen";

            if (status == "Rejected")
                return "Odbijen";

            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
