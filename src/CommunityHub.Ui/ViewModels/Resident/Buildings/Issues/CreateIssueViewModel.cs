using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Application.Services.Issues;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Buildings.Issues;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Issues
{
    public class CreateIssueViewModel : BaseViewModel
    {
        private readonly IssueService _issueService = new();
        private readonly long _userId;
        private readonly long _buildingId;

        public CreateIssueViewModel(
            long userId,
            long buildingId,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            _userId = userId;
            _buildingId = buildingId;

            SubmitCommand = new(Submit, () => IsFormValid);
            OpenSelectPriorityPageCommand = new(OpenSelectPriorityPage);
        }

        public RelayCommand SubmitCommand { get; }
        public RelayCommand OpenSelectPriorityPageCommand { get; }

        private void OpenSelectPriorityPage()
            => Push(
                new SelectIssuePriorityPage(SelectedPriority),
                onPop: value =>
                {
                    if (value is IssuePriority priority)
                        SelectedPriority = priority;
                }
            );

        private void Submit()
        {
            if (!IsFormValid)
                throw new InvalidOperationException("Invalid form can't be submitted.");

            var issue = _issueService.Create(
                new IssueDto(
                    id: 0,
                    description: Description.Trim(),
                    reportedAt: DateTime.Today,
                    priority: SelectedPriority!.Value,
                    status: IssueStatus.Open,
                    residentId: _userId,
                    resident: null,
                    buildingId: _buildingId,
                    building: null
                )
            );

            ShowAlert(
                Alert.Create(
                    "Problem prijavljen",
                    "Vaš problem je uspješno prijavljen upravniku zgrade.",
                    AlertType.Info
                )
                .AddOption("U redu", () => Pop(issue))
            );
        }

        private IssuePriority? _selectedPriority;
        public IssuePriority? SelectedPriority
        {
            get => _selectedPriority;
            set
            {
                if (_selectedPriority == value) return;
                _selectedPriority = value;
                OnPropertyChanged(nameof(SelectedPriority));
                OnPropertyChanged(nameof(IsFormValid));
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(IsFormValid));
                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsFormValid
            => !string.IsNullOrWhiteSpace(Description) && SelectedPriority is not null;
    }
}
