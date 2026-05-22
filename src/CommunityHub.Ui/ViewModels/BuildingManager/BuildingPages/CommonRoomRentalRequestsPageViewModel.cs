using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class CommonRoomRentalRequestsPageViewModel : INotifyPropertyChanged
    {
        private readonly CommonRoomRentalRequestService _rentalRequestService = new();
        private readonly BuildingMapper _buildingMapper = new();
        private CommonRoomRentalRequestDto? _selectedRentalRequest;

        private Visibility _defaultActionTextVisibility = Visibility.Visible;
        private Visibility _approvedActionTextVisibility = Visibility.Collapsed;
        private Visibility _rejectedActionTextVisibility = Visibility.Collapsed;
        private Visibility _waitingDateChangeActionTextVisibility = Visibility.Collapsed;
        private Visibility _oneDayPanelVisibility = Visibility.Collapsed;
        private Visibility _multiDayPanelVisibility = Visibility.Collapsed;

        public User User { get; }
        public Building Building { get; }
        public BuildingDto BuildingDto { get; }
        public string StreetInfo => $"{BuildingDto.Address.Street.Name} {BuildingDto.Address.StreetNumber}";
        public ObservableCollection<CommonRoomRentalRequestDto> RentalRequests { get; set; } = [];
        public ObservableCollection<DateTime> SuggestedOneDayDates { get; set; } = [];
        public ObservableCollection<SuggestedDateRangeViewModel> SuggestedMultiDayRanges { get; set; } = [];

        public CommonRoomRentalRequestDto? SelectedRentalRequest
        {
            get => _selectedRentalRequest;
            set
            {
                _selectedRentalRequest = value;
                OnPropertyChanged();
                ShowManagerAction(value);
            }
        }
        public Visibility DefaultActionTextVisibility
        {
            get => _defaultActionTextVisibility;
            set { _defaultActionTextVisibility = value; OnPropertyChanged(); }
        }
        public Visibility ApprovedActionTextVisibility
        {
            get => _approvedActionTextVisibility;
            set { _approvedActionTextVisibility = value; OnPropertyChanged(); }
        }
        public Visibility RejectedActionTextVisibility
        {
            get => _rejectedActionTextVisibility;
            set { _rejectedActionTextVisibility = value; OnPropertyChanged(); }
        }
        public Visibility WaitingDateChangeActionTextVisibility
        {
            get => _waitingDateChangeActionTextVisibility;
            set { _waitingDateChangeActionTextVisibility = value; OnPropertyChanged(); }
        }
        public Visibility OneDayPanelVisibility
        {
            get => _oneDayPanelVisibility;
            set { _oneDayPanelVisibility = value; OnPropertyChanged(); }
        }
        public Visibility MultiDayPanelVisibility
        {
            get => _multiDayPanelVisibility;
            set { _multiDayPanelVisibility = value; OnPropertyChanged(); }
        }

        public RelayCommand<DateTime> ApproveOneDaySuggestionCommand { get; }
        public RelayCommand<SuggestedDateRangeViewModel> AcceptMultiDaySuggestionCommand { get; }

        public CommonRoomRentalRequestsPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
            BuildingDto = _buildingMapper.MapToDto(building);

            RefreshRentalRequests();

            ApproveOneDaySuggestionCommand = new RelayCommand<DateTime>(day => ApproveOneDaySuggestion(day));
            AcceptMultiDaySuggestionCommand = new RelayCommand<SuggestedDateRangeViewModel>(
                range => AcceptMultiDaySuggestion(range!.StartDate, range.EndDate));
        }

        public void RefreshRentalRequests()
        {
            RentalRequests.Clear();
            var rentalRequests = _rentalRequestService.GetAllByBuildingId(Building.Id);

            foreach (var request in rentalRequests)
                RentalRequests.Add(request);
        }
        public void AutoProcessRentalRequestStatuses()
        {
            _rentalRequestService.AutoProcessRentalRequestStatuses(Building.Id);
        }
        private void ShowManagerAction(CommonRoomRentalRequestDto? request)
        {
            SetVisibility();

            if (request == null)
            {
                DefaultActionTextVisibility = Visibility.Visible;
                return;
            }

            switch (request?.Status)
            {
                case RentalRequestStatus.Approved:
                    ApprovedActionTextVisibility = Visibility.Visible;
                    break;
                case RentalRequestStatus.Rejected:
                    RejectedActionTextVisibility = Visibility.Visible;
                    break;
                case RentalRequestStatus.WaitingDateChange:
                    WaitingDateChangeActionTextVisibility = Visibility.Visible;
                    break;
                case RentalRequestStatus.Waiting:
                    HandleWaitingStatus(request);
                    break;
            }
        }
        private void SetVisibility()
        {
            DefaultActionTextVisibility = Visibility.Collapsed;
            ApprovedActionTextVisibility = Visibility.Collapsed;
            RejectedActionTextVisibility = Visibility.Collapsed;
            WaitingDateChangeActionTextVisibility = Visibility.Collapsed;
            OneDayPanelVisibility = Visibility.Collapsed;
            MultiDayPanelVisibility = Visibility.Collapsed;
        }
        private void HandleWaitingStatus(CommonRoomRentalRequestDto request)
        {
            if (request.CommonRoom!.RentalType == RentalType.OneDay)
                SuggestOneDayDates(request);
            else
                SuggestMultiDayDates(request);
        }
        private void SuggestOneDayDates(CommonRoomRentalRequestDto request)
        {
            SuggestedOneDayDates.Clear();
            var dates = _rentalRequestService.GetSuggestedOneDayDates(request, Building.Id);

            foreach (var day in dates)
                SuggestedOneDayDates.Add(day);

            OneDayPanelVisibility = Visibility.Visible;
        }
        private void SuggestMultiDayDates(CommonRoomRentalRequestDto request)
        {
            SuggestedMultiDayRanges.Clear();
            var ranges = _rentalRequestService.GetSuggestedMultiDayRanges(request, Building.Id);

            foreach (var range in ranges)
                SuggestedMultiDayRanges.Add(new SuggestedDateRangeViewModel(range.StartDate, range.EndDate));

            MultiDayPanelVisibility = Visibility.Visible;
        }
        public void ApproveOneDaySuggestion(DateTime day)
        {
            if (SelectedRentalRequest == null)
                return;

            var result = MessageBox.Show("Da li želite da promenite datum rezervacije?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            _rentalRequestService.ApproveOneDaySuggestion(SelectedRentalRequest, day);
            AutoProcessRentalRequestStatuses();
            RefreshRentalRequests();
            OneDayPanelVisibility = Visibility.Collapsed;
            SelectedRentalRequest = null;
        }
        public void AcceptMultiDaySuggestion(DateTime proposedStartDate, DateTime proposedEndDate)
        {
            if (SelectedRentalRequest == null)
                return;

            var result = MessageBox.Show("Da li želite da promenite opseg rezervacije", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No) return;

            _rentalRequestService.AcceptMultiDaySuggestion(SelectedRentalRequest, proposedStartDate, proposedEndDate);
            AutoProcessRentalRequestStatuses();
            RefreshRentalRequests();
            MultiDayPanelVisibility = Visibility.Collapsed;
            SelectedRentalRequest = null;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SuggestedDateRangeViewModel
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public string DisplayText => $"{StartDate:dd.MM.yyyy.} - {EndDate:dd.MM.yyyy.}";

        public SuggestedDateRangeViewModel(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}