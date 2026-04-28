using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CommonRoom;
using CommunityHub.Application.Domain.CommonRoomRentalRequest;
using CommunityHub.Ui.Helpers.Alerts;
using CommunityHub.Ui.Helpers.Navigation;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace CommunityHub.Ui.Views.Resident.Buildings.CommonRooms.Details.Requests
{
    public enum RentalRequestValidationResult
    {
        Valid,
        PendingRequestExists
    }

    public partial class RequestCommonRoomRentalPage : MainNavigatorConsumerPage, INotifyPropertyChanged, IAlertConsumer
    {
        private readonly long _userId;
        private readonly CultureInfo _culture = new("sr-Latn-RS");
        private readonly DateTime _today = DateTime.Today;

        private readonly UserRepository _userRepository = new();
        private  readonly CommonRoomRentalRequestRepository _rentalRequestRepository = new();

        public CommonRoom CommonRoom { get; }

        public RequestCommonRoomRentalPage(long userId, CommonRoom commonRoom)
        {
            _userId = userId;
            CommonRoom = commonRoom;

            InitializeComponent();
            DataContext = this;
        }

        // The start date can't be in the past.
        public DateTime MinStartDate => _today;

        // The start date can't be after the end date.
        public DateTime? MaxStartDate => SelectedEndDate;

        // The end date can't be before the start date
        // (or in the past if the start date hasn't been
        // selected yet).
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
        private void ShowSuccessAlert()
        {
            if (this is IAlertConsumer consumer)
                consumer.ShowAlert(
                    Alert.Create(
                        "Zahtjev podnesen",
                        "Vaš zahtjev je uspješno podnesen.",
                        AlertType.Info
                    )
                    .AddOption("U redu", () => Pop())
                );
        }

        private void ShowRentalRequestValidationResult(RentalRequestValidationResult result)
        {
            if (this is not IAlertConsumer consumer)
                return;

            switch (result)
            {
                case RentalRequestValidationResult.PendingRequestExists:
                        consumer.ShowAlert(
                              Alert.Create(
                                  "Greška",
                                  "Nije moguće kreirati zahtjev za iznajmljivanje ove zajedničke prostorije zato što ste već imate zahtjev za ovu zajedničku prostoriju koji je još uvijek u fazi obrade.",
                                  AlertType.Error
                              )
                              .AddOption("U redu")
                          );
                    break;

            }
        }

        RentalRequestValidationResult ValidateRentalRequest()
        {
            if (_rentalRequestRepository.HasPendingRequest(_userId, CommonRoom.Id))
                return RentalRequestValidationResult.PendingRequestExists;

            return RentalRequestValidationResult.Valid;
        }

        CommonRoomRentalRequest? CreateRentalRequest()
        {
            var resident = _userRepository.Get(_userId);

            if (resident is null) return null;

           return new CommonRoomRentalRequest(
                resident: resident,
                commonRoom: CommonRoom,
                requestedStartDate: SelectedStartDate!.Value,
                requestedEndDate: SelectedEndDate!.Value
            );
        }

        private void CreateRentalRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var request = CreateRentalRequest();

            if (request is null) return;

            var validationResult = ValidateRentalRequest();

            if (validationResult == RentalRequestValidationResult.Valid)
            {
                _rentalRequestRepository.Create(request);
                ShowSuccessAlert();
                return;
            }

            ShowRentalRequestValidationResult(validationResult);
        }

        private void BackButton_Click(object sender, MouseButtonEventArgs e) => Pop();


        private void ToggleOverlayVisibility(FrameworkElement overlay)
            =>
                overlay.Visibility = overlay.Visibility switch
                {
                    Visibility.Visible => Visibility.Collapsed,
                    Visibility.Collapsed => Visibility.Visible,
                    _ => Visibility.Visible
                };

        private void ToggleSelectDateRangeMenuButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(SelectDateRangeOverlay);

        private void ToggleStartDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(StartDatePicker);

        private void ToggleEndDatePickerButton_Click(object sender, EventArgs e)
            => ToggleOverlayVisibility(EndDatePicker);


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
