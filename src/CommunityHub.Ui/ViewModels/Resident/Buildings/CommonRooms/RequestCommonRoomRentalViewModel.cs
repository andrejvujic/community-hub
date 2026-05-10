using CommunityHub.Application.Domain.Buildings.CommonRooms;
using CommunityHub.Application.Domain.Buildings.CommonRooms.RentalRequests;
using CommunityHub.Application.Modules.Buildings.Core.Services.CommonRooms;
using System.ComponentModel;
using System.Globalization;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class RequestCommonRoomRentalViewModel : INotifyPropertyChanged
    {
        private readonly CultureInfo _culture = new("sr-Latn-RS");
        private readonly DateTime _today = DateTime.Today;

        private readonly CommonRoomRentalRequestService _commonRoomRentalRequestService = new();

        public long UserId { get; }
        public CommonRoom CommonRoom { get; }

        public RequestCommonRoomRentalViewModel(long userId, CommonRoom commonRoom)
        {
            UserId = userId;
            CommonRoom = commonRoom;
        }

        public RentalRequestValidationResult ValidateRentalRequest()
            => _commonRoomRentalRequestService.ValidateRentalRequest(UserId, CommonRoom.Id);

        public void CreateRentalRequest()
            => _commonRoomRentalRequestService.Create(
                new CommonRoomRentalRequest(
                    residentId: UserId,
                    commonRoomId: CommonRoom.Id,
                    requestedStartDate: SelectedStartDate!.Value,
                    requestedEndDate: SelectedEndDate!.Value
                )    
            );

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
