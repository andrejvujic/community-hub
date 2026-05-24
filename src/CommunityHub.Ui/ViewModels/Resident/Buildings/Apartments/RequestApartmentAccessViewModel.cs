using CommunityHub.Application.Domain;
using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Apartments
{
    public class RequestApartmentAccessViewModel : BaseViewModel
    {
        private readonly ApartmentAccessRequestService _apartmentAccessRequestService = new();
        private readonly ApartmentService _apartmentService = new();

        public long UserId { get; }
        public BuildingDto Building { get; }

        public RequestApartmentAccessViewModel(long userId, BuildingDto building, MainNavigationConsumerPage page) : base(page) {
            UserId = userId;
            Building = building;
        }

        public int ApartmentNumber
        {
            get
            {
                if (int.TryParse(ApartmentNumberText, out int apartmentNumber))
                    return apartmentNumber;
                return 0;
            }
        }

        public bool IsApartmentNumberValid { get => ApartmentNumber > 0; }

        private string _apartmentNumberText = string.Empty;
        public string ApartmentNumberText
        {
            get => _apartmentNumberText;
            set
            {
                if (value == _apartmentNumberText) return;
                _apartmentNumberText = value;
                OnPropertyChanged(nameof(ApartmentNumberText));
                OnPropertyChanged(nameof(ApartmentNumber));
                OnPropertyChanged(nameof(IsApartmentNumberValid));
            }
        }

        public AccessRequestValidationResult ValidateAccessRequest()
            => _apartmentAccessRequestService.ValidateAccessRequest(
                    UserId,
                    Building.Id,
                    ApartmentNumber
                );

        public void CreateAccessRequest()
        {
            var apartmentId = _apartmentService.FindIdByBuildingAndNumber(
                Building.Id,
                ApartmentNumber
            );

            if (apartmentId is null)
                throw new ArgumentException("ApartmentId can't be null.");

            _apartmentAccessRequestService.Create(
                new ApartmentAccessRequestDto(
                    applicantId: UserId,
                    apartmentId: apartmentId!.Value,
                    status: Status.Waiting,
                    creationDate: DateTime.Now
                )
            );
        }

        public void SubmitAccessRequest()
        {
            var result = ValidateAccessRequest();
            if (result is AccessRequestValidationResult.Valid)
            {
                CreateAccessRequest();
                ShowSuccessAlert();
                return;
            }

            ShowValidationResult(result);
        }

        private void ShowSuccessAlert()
            => ShowAlert(
                Alert.Create(
                    "Zahtjev podnesen",
                    "Vaš zahtjev je uspješno podnesen. Sve svoje zahtjeve možete pogledati na početnom ekranu.",
                    AlertType.Info
                )
                .AddOption("U redu", () => PopAll())
            );

        private void ShowValidationResult(AccessRequestValidationResult result)
        {
            switch (result)
            {
                case AccessRequestValidationResult.ApartmentNotFound:
                    ShowAlert(
                        Alert.Create(
                            "Greška",
                            $"Stan broj {ApartmentNumber} ne postoji u zgradi {Building.DisplayAddress}. Provjerite uneseni broj stana i pokušajte ponovo.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;
                case AccessRequestValidationResult.ApartmentOccupied:
                    ShowAlert(
                        Alert.Create(
                            "Upozorenje",
                            $"Da li ste sigurni da ste unijeli ispravan broj stana? Već postoji član koji je pridružen stanu broj {ApartmentNumber} u zgradi {Building.DisplayAddress}.",
                            AlertType.Warning
                        )
                        .AddOption("Otkaži")
                        .AddOption("Nastavi", () => {
                            CreateAccessRequest();
                            ShowSuccessAlert();
                        })
                    );
                    break;
                case AccessRequestValidationResult.AlreadyMember:
                    ShowAlert(
                        Alert.Create(
                            "Greška",
                            $"Nije moguće kreirati zahtjev za pridruženje stanu broj {ApartmentNumber} u zgradi {Building.DisplayAddress} zato što ste već učlanjeni u ovaj stan.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;
                case AccessRequestValidationResult.PendingRequestExists:
                    ShowAlert(
                        Alert.Create(
                            "Greška",
                            $"Nije moguće kreirati zahtjev za pridruženje stanu broj {ApartmentNumber} u zgradi {Building.DisplayAddress} zato što već imate zahtjev za ovaj stan koji je još uvijek u fazi obrade.",
                            AlertType.Error
                        )
                        .AddOption("U redu")
                    );
                    break;
            }
        }
    }
}
