using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Application.Services.Issues;
using CommunityHub.Ui.Helpers.Navigation;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Issues
{
    public class IssuesViewModel : BaseViewModel
    {
        private readonly IssueService _issueService = new();
        private readonly long _userId;
        private readonly long _buildingId;

        public ObservableCollection<IssueDto> ReportedIssues { get; } = new();
        public bool HasReportedIssues => ReportedIssues.Count > 0;

        public IssuesViewModel(
            long userId,
            long buildingId,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _userId = userId;
            _buildingId = buildingId;
        }

        public void UpdateIssues()
        {
            ReportedIssues.Clear();

            var issues = _issueService.GetAllByBuildingAndResident(_buildingId, _userId);
            foreach (var issue in issues)
                ReportedIssues.Add(issue);

            OnPropertyChanged(nameof(HasReportedIssues));
        }
    }
}
