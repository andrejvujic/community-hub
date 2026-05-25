using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Application.Services.Issues;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Issues;
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

            OpenCreateIssueCommand = new(OpenCreateIssuePage);
            OpenIssueDetailsCommand = new(OpenIssueDetailsPage);
        }

        public RelayCommand OpenCreateIssueCommand { get; }
        public RelayCommand<IssueDto> OpenIssueDetailsCommand { get; }

        private void OpenCreateIssuePage()
            => Push(
                new CreateIssuePage(_userId, _buildingId),
                onPop: value =>
                {
                    if (value is IssueDto)
                        UpdateIssues();
                }
            );

        private void OpenIssueDetailsPage(IssueDto? issue)
        {
            if (issue is null) return;

            Push(
                new IssueDetailsPage(issue),
                onPop: _ => UpdateIssues()
            );
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
