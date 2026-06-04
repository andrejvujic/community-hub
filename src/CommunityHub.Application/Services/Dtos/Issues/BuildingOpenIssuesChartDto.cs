namespace CommunityHub.Application.Services.Dtos.Issues
{
    public class BuildingOpenIssuesChartDto
    {
        public string BuildingInfo { get; set; } = string.Empty;
        public int OpenIssuesCount { get; set; }
    }
}