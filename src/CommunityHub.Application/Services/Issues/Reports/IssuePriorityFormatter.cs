using CommunityHub.Application.Domain.Issues;

namespace CommunityHub.Application.Services.Issues.Reports
{
    public static class IssuePriorityFormatter
    {
        public static string Format(IssuePriority priority)
        {
            return priority switch
            {
                IssuePriority.Urgent => "Potrebno hitno rešavanje",
                IssuePriority.CanWait => "Može da čeka",
                IssuePriority.ShouldBeResolvedSoon => "Treba rešiti u narednom periodu",
                _ => "Nepoznato"
            };
        }
    }
}