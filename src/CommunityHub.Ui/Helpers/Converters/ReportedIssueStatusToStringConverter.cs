using CommunityHub.Application.Domain.ReportedIsssue;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class ReportedIssueStatusToStringConverter : IValueConverter
    {
        private readonly string _openText = "Nerešen";
        private readonly string _potentiallyResolvedText = "Potencijalno rešen";
        private readonly string _resolvedText = "Rešen";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ReportedIssueStatus status) return null;

            switch (status)
            {
                case ReportedIssueStatus.Open:
                    return _openText;
                case ReportedIssueStatus.PotentiallyResolved:
                    return _potentiallyResolvedText;
                case ReportedIssueStatus.Resolved:
                    return _resolvedText;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
