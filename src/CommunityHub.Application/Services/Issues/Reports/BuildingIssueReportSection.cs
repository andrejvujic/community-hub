using CommunityHub.Application.Services.Dtos.Issues;

namespace CommunityHub.Application.Services.Issues.Reports
{
    public class BuildingIssueReportSection
    {
        public string BuildingName { get; set; } = string.Empty;
        public List<IssueDto> Items { get; set; } = [];
    }
}