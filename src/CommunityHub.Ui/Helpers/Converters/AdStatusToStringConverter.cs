using CommunityHub.Application.Domain.Buildings.Ads;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class AdStatusToStringConverter : IValueConverter
    {
        private readonly string _activeText = "Aktivan";
        private readonly string _archivedText = "Arhiviran";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not AdStatus status) return null;

            switch (status)
            {
                case AdStatus.Active:
                    return _activeText;
                case AdStatus.Archived:
                    return _archivedText;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
