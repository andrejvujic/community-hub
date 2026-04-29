using CommunityHub.Application.Domain.Buildings.ReportedIssues;
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
            if (value is not IssueStatus status) return null;

            switch (status)
            {
                case IssueStatus.Open:
                    return _openText;
                case IssueStatus.PotentiallyResolved:
                    return _potentiallyResolvedText;
                case IssueStatus.Resolved:
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
