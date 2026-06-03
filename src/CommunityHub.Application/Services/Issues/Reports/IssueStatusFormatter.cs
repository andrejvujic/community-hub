using CommunityHub.Application.Domain.Issues;

namespace CommunityHub.Application.Services.Issues.Reports
{
    public static class IssueStatusFormatter
    {
        public static string Format(IssueStatus status)
        {
            return status switch
            {
                IssueStatus.Open => "Nerešen",
                IssueStatus.PotentiallyResolved => "Potencijalno rešen",
                IssueStatus.Resolved => "Rešen",
                _ => "Nepoznato"
            };
        }
    }
}