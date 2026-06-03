using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Services.CommonRooms;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityHub.Ui.Helpers.BuildingManagerAlerts;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class CommonRoomsPageViewModel : INotifyPropertyChanged
    {
        private readonly BuildingMapper _buildingMapper = new();
        private readonly CommonRoomService _commonRoomService = new();
        private readonly CommonRoomRentalRequestService _commonRoomRentalRequestService = new();

        public User User { get; }
        public Building Building { get; }
        public BuildingDto BuildingDto { get; }
        public ObservableCollection<CommonRoomDto> CommonRooms { get; } = [];
        public ObservableCollection<DateTime> ReservedDates { get; } = [];

        public CommonRoomsPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
            BuildingDto = _buildingMapper.MapToDto(building);

            LoadCommonRooms();
            CancelCommand = new RelayCommand<object>(_ => Cancel());
            ConfirmCommand = new RelayCommand<object>(_ => Confirm(), _ => CanConfirm());
        }

        public RelayCommand<object> CancelCommand { get; }
        public RelayCommand<object> ConfirmCommand { get; }

        private CommonRoomDto? _selectedCommonRoom;
        public CommonRoomDto? SelectedCommonRoom
        {
            get => _selectedCommonRoom;
            set
            {
                _selectedCommonRoom = value;
                OnPropertyChanged();
                LoadReservedDatesForSelectedRoom();
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                if (ShowValidationErrors)
                    ValidateName();
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
                if (ShowValidationErrors)
                    ValidateDescription();
            }
        }

        private int _selectedFloor = 1;
        public int SelectedFloor
        {
            get => _selectedFloor;
            set
            {
                _selectedFloor = value;
                OnPropertyChanged();
            }
        }

        private int _selectedRentalTypeIndex = 0;
        public int SelectedRentalTypeIndex
        {
            get => _selectedRentalTypeIndex;
            set
            {
                _selectedRentalTypeIndex = value;
                OnPropertyChanged();
            }
        }

        private bool _isCalendarVisible;
        public bool IsCalendarVisible
        {
            get => _isCalendarVisible;
            set
            {
                _isCalendarVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _showValidationErrors;
        public bool ShowValidationErrors
        {
            get => _showValidationErrors;
            set
            {
                _showValidationErrors = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNameErrorVisible));
                OnPropertyChanged(nameof(IsDescriptionErrorVisible));
            }
        }

        private string _nameError = string.Empty;
        public string NameError
        {
            get => _nameError;
            set
            {
                _nameError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNameErrorVisible));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsNameErrorVisible => ShowValidationErrors && !string.IsNullOrWhiteSpace(NameError);

        private string _descriptionError = string.Empty;
        public string DescriptionError
        {
            get => _descriptionError;
            set
            {
                _descriptionError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDescriptionErrorVisible));
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsDescriptionErrorVisible => ShowValidationErrors && !string.IsNullOrWhiteSpace(DescriptionError);

        public string StreetInfo
        {
            get => $"{BuildingDto.Address.Street.Name} {BuildingDto.Address.StreetNumber}";
        }
        public int FloorMaximum => Building.FloorCount;

        public void LoadCommonRooms()
        {
            CommonRooms.Clear();

            var commonRooms = _commonRoomService.GetAllByBuilding(Building.Id);

            foreach (var room in commonRooms)
                CommonRooms.Add(room);
        }
 
        public bool ValidateForm()
        {
            ShowValidationErrors = true;
            ValidateName();
            ValidateDescription();

            return string.IsNullOrWhiteSpace(NameError) && string.IsNullOrWhiteSpace(DescriptionError);
        }
        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(Name))
                NameError = "Niste uneli naziv prostorije.";
            else
                NameError = string.Empty;
        }
        private void ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(Description))
                DescriptionError = "Niste uneli opis prostorije.";
            else
                DescriptionError = string.Empty;
        }
        public void CreateCommonRoom()
        { 
            _commonRoomService.Create(Building.Id, Name.Trim(), Description.Trim(), SelectedFloor, SelectedRentalTypeIndex);
        }
        public void ResetForm()
        {
            Name = string.Empty;
            Description = string.Empty;
            SelectedFloor = 1;
            SelectedRentalTypeIndex = 0;
            ShowValidationErrors = false;
            NameError = string.Empty;
            DescriptionError = string.Empty;
        }
        private void LoadReservedDatesForSelectedRoom()
        {
            ReservedDates.Clear();

            if (SelectedCommonRoom == null)
            {
                IsCalendarVisible = false;
                return;
            }

            var reservedDates = _commonRoomRentalRequestService.GetReservedDates(
                SelectedCommonRoom.Id,
                Building.Id);

            foreach (var date in reservedDates)
                ReservedDates.Add(date);

            IsCalendarVisible = true;
        }
        private void Cancel()
        {
            var dialogService = new DialogService();

            var confirm = dialogService.ShowConfirmation(
                "Potvrda",
                "Da li želite da otkažete kreiranje zajedničke prostorije?");
            
            if (confirm)
                ResetForm();
        }
        private void Confirm()
        {
            if (!ValidateForm())
                return;

            var dialogService = new DialogService();

            var confirm = dialogService.ShowConfirmation(
                "Potvrda",
                "Da li želite da kreirate zajedničku prostoriju?");
            if (!confirm)
                return;

            CreateCommonRoom();
            ResetForm();
            LoadCommonRooms();

            dialogService.ShowInfo("Čestitamo", "Zajednička prostorija je uspešno kreirana.");
        }
        private bool CanConfirm()
        {
            return !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Description);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}