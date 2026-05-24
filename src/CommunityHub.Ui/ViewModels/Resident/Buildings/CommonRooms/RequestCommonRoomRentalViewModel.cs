using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Helpers.Navigation;
using System.Globalization;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class RequestCommonRoomRentalViewModel : BaseViewModel
    {
        private readonly CultureInfo _culture = new("sr-Latn-RS");
        private readonly DateTime _today = DateTime.Today;

        private readonly CommonRoomRentalRequestService _commonRoomRentalRequestService = new();

        public long UserId { get; }
        public CommonRoomDto CommonRoom { get; }

        public RequestCommonRoomRentalViewModel(long userId, CommonRoomDto commonRoom, MainNavigationConsumerPage page) : base(page)
        {
            UserId = userId;
            CommonRoom = commonRoom;

            SubmitCommand = new(SubmitRentalRequest, () => IsSelectedDateRangeValid);
        }

        public RelayCommand SubmitCommand { get; }

        private RentalRequestValidationResult ValidateRentalRequest()
            => _commonRoomRentalRequestService.ValidateRentalRequest(UserId, CommonRoom.Id);

        private void CreateRentalRequest()
            => _commonRoomRentalRequestService.Create(
                new CommonRoomRentalRequestDto(
                    residentId: UserId,
                    commonRoomId: CommonRoom.Id,
                    requestedStartDate: SelectedStartDate!.Value,
                    requestedEndDate: SelectedEndDate!.Value,
                    status: RentalRequestStatus.Waiting
                )    
            );

        private void SubmitRentalRequest()
        {
            var result = ValidateRentalRequest();

            if (result is RentalRequestValidationResult.Valid)
            {
                CreateRentalRequest();
                ShowSuccessAlert();
                return;
            }

            ShowValidationResult(result);
        }

        private void ShowSuccessAlert()
            => ShowAlert(
                Alert.Create(
                    "Zahtjev podnesen",
                    "Vaš zahtjev je uspješno podnesen.",
                    AlertType.Info
                )
                .AddOption("U redu", () => Pop())
            );

        private void ShowValidationResult(RentalRequestValidationResult result)
        {
            if (result is RentalRequestValidationResult.PendingRequestExists)
                ShowAlert(
                    Alert.Create(
                        "Greška",
                        "Nije moguće kreirati zahtjev za iznajmljivanje ove zajedničke prostorije zato što ste već imate zahtjev za ovu zajedničku prostoriju koji je još uvijek u fazi obrade.",
                        AlertType.Error
                    )
                    .AddOption("U redu")
                );
        }

        public DateTime MinStartDate => _today;
        public DateTime? MaxStartDate => SelectedEndDate;
        public DateTime MinEndDate => SelectedStartDate ?? _today;


        private DateTime? _selectedStartDate;
        public DateTime? SelectedStartDate
        {
            get => _selectedStartDate;
            set
            {
                if (_selectedStartDate == value) return;
                _selectedStartDate = value is null ? null : value.Value.Date;

                OnPropertyChanged(nameof(SelectedStartDate));
                OnPropertyChanged(nameof(SelectedStartDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));

                OnPropertyChanged(nameof(MinEndDate));

                OnPropertyChanged(nameof(IsSelectedDateRangeValid));

                SubmitCommand.RaiseCanExecuteChanged();
            }
        }
        public string SelectedStartDateText
            => SelectedStartDate is null ?
                "Izaberite..." :
                SelectedStartDate.Value.ToString("d. MMMM", _culture);


        private DateTime? _selectedEndDate;
        public DateTime? SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                if (_selectedEndDate == value) return;
                _selectedEndDate = value is null ? null : value.Value.Date;

                OnPropertyChanged(nameof(SelectedEndDate));
                OnPropertyChanged(nameof(SelectedEndDateText));
                OnPropertyChanged(nameof(SelectedDateRangeText));

                OnPropertyChanged(nameof(MaxStartDate));

                OnPropertyChanged(nameof(IsSelectedDateRangeValid));

                SubmitCommand.RaiseCanExecuteChanged();
            }
        }

        public string SelectedEndDateText
            => SelectedEndDate is null ?
                "Izaberite..." :
                SelectedEndDate.Value.ToString("d. MMMM", _culture);

        public string SelectedDateRangeText
        {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return "Izaberite...";
                return $"{SelectedStartDateText} — {SelectedEndDateText}";
            }
        }

        public bool IsSelectedDateRangeValid
        {
            get
            {
                if (SelectedStartDate is null || SelectedEndDate is null) return false;
                if (SelectedStartDate.Value < _today) return false;
                if (SelectedEndDate.Value < SelectedStartDate.Value) return false;

                return true;
            }
        }

    }
}
