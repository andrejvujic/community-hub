using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomRentalRequestDetailsViewModel : BaseViewModel
    {
        private readonly CommonRoomRentalRequestService _commonRentalRequestService = new();

        public CommonRoomRentalRequestDto Request { get; private set; }

        public CommonRoomRentalRequestDetailsViewModel(
            CommonRoomRentalRequestDto request, 
            IMainNavigationConsumer navigationConsumer
        ) : base(navigationConsumer)
        {
            Request = request;

            CancelCommand = new(CancelRequest, () => Request.IsPending);
            AcceptProposedDateRangeCommand = new(AcceptProposedDateRange, () => Request.HasProposedDateRange);
        }

        public RelayCommand CancelCommand { get; }
        public RelayCommand AcceptProposedDateRangeCommand { get; }

        private void CancelRequest()
        { 
            ShowAlert(
                Alert.Create(
                    title: "Upozorenje",
                    content: "Da li ste sigurni da želite da odbijete predloženi opseg datuma? Ovo će rezultovati u trajnom brisanju vašeg zahtjeva.",
                    type: AlertType.Warning
                )
                .AddOption("Otkaži")
                .AddOption("Nastavi", () => {
                    _commonRentalRequestService.Cancel(Request);
                    Pop();
                })
            );
        }

        private void AcceptProposedDateRange()
        {
            Request = _commonRentalRequestService.AcceptProposedDateRange(Request);
            OnPropertyChanged(nameof(Request));

            ShowAlert(
                Alert.Create(
                    title: "Informacija",
                    content: "Uspješno ste prihvatili predloženi opseg datuma. Vaš zahtjev je ponovo u fazi obrade i biće pregledan od strane upravnika.",
                    type: AlertType.Info
                )
                .AddOption("U redu")
            );
        }
    }
}
