using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomRentalRequestDetailsViewModel : BaseViewModel
    {
        private readonly CommonRoomRentalRequestService _commonRentalRequestService = new();

        public CommonRoomRentalRequestDto Request { get; private set; }

        public CommonRoomRentalRequestDetailsViewModel(CommonRoomRentalRequestDto request, MainNavigationConsumerPage page) : base(page)
        {
            Request = request;
        }

        public void CancelRequest()
            => _commonRentalRequestService.Cancel(Request);

        public void AcceptProposedRequestDateRange()
        {
            Request = _commonRentalRequestService.AcceptProposedDateRange(Request);
            OnPropertyChanged(nameof(Request));
        }

        public void ConfirmCancelRequest()
            => ShowAlert(
                Alert.Create(
                    title: "Upozorenje",
                    content: "Da li ste sigurni da želite da odbijete predloženi opseg datuma? Ovo će rezultovati u trajnom brisanju vašeg zahtjeva.",
                    type: AlertType.Warning
                )
                .AddOption("Otkaži")
                .AddOption("Nastavi", () => {
                    CancelRequest();
                    Pop();
                })
            );

        public void AcceptProposedDateRange()
        {
            AcceptProposedRequestDateRange();
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
