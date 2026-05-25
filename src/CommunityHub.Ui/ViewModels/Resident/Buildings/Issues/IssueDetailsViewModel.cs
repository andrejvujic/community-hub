using CommunityHub.Application.Domain.Issues;
using CommunityHub.Application.Services.Dtos.Issues;
using CommunityHub.Application.Services.Issues;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Issues
{
    public class IssueDetailsViewModel : BaseViewModel
    {
        private readonly IssueService _issueService = new();

        public IssueDto Issue { get; private set; }
        public bool CanConfirmResolution => Issue.Status is IssueStatus.PotentiallyResolved;

        public IssueDetailsViewModel(
            IssueDto issue,
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            Issue = issue;
            ConfirmResolutionCommand = new(ConfirmResolution, () => CanConfirmResolution);
        }

        public RelayCommand ConfirmResolutionCommand { get; }

        private void ConfirmResolution()
            => ShowAlert(
                Alert.Create(
                    "Potvrda rješavanja",
                    "Da li ste sigurni da želite da potvrdite rješavanje ovog problema?",
                    AlertType.Warning
                )
                .AddOption("Otkaži")
                .AddOption(
                    "Potvrdi",
                    () =>
                    {
                        Issue = _issueService.ConfirmResolution(Issue);
                        OnPropertyChanged(nameof(Issue));
                        OnPropertyChanged(nameof(CanConfirmResolution));
                        ConfirmResolutionCommand.RaiseCanExecuteChanged();
                    }
                )
            );
    }
}
