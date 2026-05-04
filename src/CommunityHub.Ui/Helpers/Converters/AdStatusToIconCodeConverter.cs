using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class AdStatusToIconCodeConverter : IValueConverter
    {
        private readonly string _activeIconCode = "\uf058";
        private readonly string _archivedIconCode = "\uf187";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not AdStatus status) return null;

            switch (status)
            {
                case AdStatus.Active:
                    return _activeIconCode;
                case AdStatus.Archived:
                    return _archivedIconCode;   
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
