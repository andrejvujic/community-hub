using CommunityHub.Application.Domain.Issues;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class IssuePriorityToStringConverter : IValueConverter
    {

        private readonly string _urgentText = "Potrebno hitno rešavanje";
        private readonly string _canWaitText = "Može da čeka";
        private readonly string _shouldBeResolvedSoonText = "Treba rešiti u narednom periodu";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? defaultText = parameter is string parameterValue ? parameterValue : null;

            if (value is not IssuePriority priority) return defaultText;

            switch (priority)
            {
                case IssuePriority.Urgent:
                    return _urgentText;
                case IssuePriority.CanWait:
                    return _canWaitText;
                case IssuePriority.ShouldBeResolvedSoon:
                    return _shouldBeResolvedSoonText;
            }

            return defaultText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
