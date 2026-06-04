using CommunityHub.Application.Domain.Issues;
using System.Globalization;
using System.Windows.Data;

namespace CommunityHub.Ui.Helpers.Converters
{
    public class IssueStatusToIconCodeConverter : IValueConverter
    {
        private const string _openIconCode = "\uf057";
        private const string _potentiallyResolvedIconCode = "\uf017";
        private const string _resolvedIconCode = "\uf058";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IssueStatus status)
                return null;

            return status switch
            {
                IssueStatus.Open => _openIconCode,
                IssueStatus.PotentiallyResolved => _potentiallyResolvedIconCode,
                IssueStatus.Resolved => _resolvedIconCode,
                _ => null
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
