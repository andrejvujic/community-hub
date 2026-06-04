using CommunityHub.Application.Domain.Issues;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class IssuePriorityToIconCodeConverter : IValueConverter
    {
        private const string _canWaitIconCode = "\uf017";
        private const string _shouldBeResolvedSoonIconCode = "\uf071";
        private const string _urgentIconCode = "\uf06a";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IssuePriority priority)
                return null;

            return priority switch
            {
                IssuePriority.CanWait => _canWaitIconCode,
                IssuePriority.ShouldBeResolvedSoon => _shouldBeResolvedSoonIconCode,
                IssuePriority.Urgent => _urgentIconCode,
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
