
using CommunityHub.Application.Domain.ReportedIsssue;
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
            if (value is not ReportedIssuePriority priority) return null;

            switch (priority)
            {
                case ReportedIssuePriority.Urgent:
                    return _urgentText;
                case ReportedIssuePriority.CanWait:
                    return _canWaitText;
                case ReportedIssuePriority.ShouldBeResolvedSoon:
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
