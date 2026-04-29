
using CommunityHub.Application.Domain.Buildings.ReportedIssues;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class ReportedIssuePriorityToStringConverter : IValueConverter
    {

        private readonly string _urgentText = "Potrebno hitno rešavanje";
        private readonly string _canWaitText = "Može da čeka";
        private readonly string _shouldBeResolvedSoonText = "Treba rešiti u narednom periodu";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IssuePriority priority) return null;

            switch (priority)
            {
                case IssuePriority.Urgent:
                    return _urgentText;
                case IssuePriority.CanWait:
                    return _canWaitText;
                case IssuePriority.ShouldBeResolvedSoon:
                    return _shouldBeResolvedSoonText;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
