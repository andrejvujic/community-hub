using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Application.Services.Issues;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using CommunityHub.Ui.Helpers.BuildingManagerAlerts;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class ReportedIssuesPageViewModel
    {
        private readonly BuildingMapper _buildingMapper = new();
        private readonly IssueService _issueService = new();

        public User User { get; }
        public Building Building { get; }
        public BuildingDto BuildingDto { get; }
        public ObservableCollection<IssueDto> ReportedIssues { get; } = [];
        public RelayCommand<IssueDto> SolveIssueCommand { get; }

        public ReportedIssuesPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
            BuildingDto = _buildingMapper.MapToDto(building);

            SolveIssueCommand = new RelayCommand<IssueDto>(SolveIssue);
            LoadReportedIssues();
        }
        public string StreetInfo
        {
            get => $"{BuildingDto.Address.Street.Name} {BuildingDto.Address.StreetNumber}";
        }
        public void LoadReportedIssues()
        {
            ReportedIssues.Clear();

            var issues = _issueService.GetAllByBuilding(Building.Id);

            foreach (var issue in issues)
                ReportedIssues.Add(issue);
        }
        public void MarkIssueAsResolved(IssueDto issue)
        {
            _issueService.MarkIssueAsResolved(issue);
            LoadReportedIssues();
        }
        private void SolveIssue(IssueDto? issue)
        {
            if (issue == null)
                return;

            var dialogService = new DialogService();

            var confirm = dialogService.ShowConfirmation(
                "Potvrda",
                "Da li želite da označite ovaj problem kao rešen?");

            if (!confirm)
                return;

            MarkIssueAsResolved(issue);
        }
    }
}