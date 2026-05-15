using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Services.Dtos.Buildings;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace CommunityHub.Ui.ViewModels.BuildingManager.BuildingPages
{
    public class ApartmentAccessRequestPageViewModel : INotifyPropertyChanged
    {
        private readonly BuildingMapper _buildingMapper = new();
        private readonly ApartmentAccessRequestService _apartmentAccessRequestService = new();
        public User User { get; }
        public Building Building { get; }
        public BuildingDto BuildingDto { get; }
        public ObservableCollection<ApartmentAccessRequestDto> ApartmentAccessRequests { get; } = [];

        public ApartmentAccessRequestPageViewModel(User user, Building building)
        {
            User = user;
            Building = building;
            BuildingDto = _buildingMapper.MapToDto(building);
        }

        public string StreetInfo
        {
            get => $"{BuildingDto.Address.Street.Name} {BuildingDto.Address.StreetNumber}";
        }

        private ApartmentAccessRequestDto? _selectedRequest;
        public ApartmentAccessRequestDto? SelectedRequest
        {
            get => _selectedRequest;
            set
            {
                if (_selectedRequest != value)
                {
                    _selectedRequest = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsChangeStatusEnabled));
                    Comment = string.Empty;
                    ChangeStatusSelectedIndex = 0;
                }
            }
        }

        private int _changeStatusSelectedIndex = 0;
        public int ChangeStatusSelectedIndex
        {
            get => _changeStatusSelectedIndex;
            set
            {
                if (_changeStatusSelectedIndex != value)
                {
                    _changeStatusSelectedIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsCommentEnabled));
                    
                    if (!IsCommentEnabled)
                        Comment = string.Empty;

                }
            }
        }

        private string _comment = string.Empty;
        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsChangeStatusEnabled => SelectedRequest != null;
        public bool IsCommentEnabled => ChangeStatusSelectedIndex == 2;

        private int _statusFilterSelectedIndex = 0;
        public int StatusFilterSelectedIndex
        {
            get => _statusFilterSelectedIndex;
            set
            {
                if (_statusFilterSelectedIndex != value)
                {
                    _statusFilterSelectedIndex = value;
                    OnPropertyChanged();

                    SelectedRequest = null;
                    LoadAccessRequests(GetSelectedFilterStatus());
                }
            }
        }
        public void LoadAccessRequests(Status? status)
        {
            ApartmentAccessRequests.Clear();

            var requests = _apartmentAccessRequestService.GetByUserAndBuilding(User.Id, Building.Id, status);

            foreach(var request in requests)
                ApartmentAccessRequests.Add(request);
        }
        public void ResetSelectionAndForm()
        {
            SelectedRequest = null;
            ChangeStatusSelectedIndex = 0;
            Comment = string.Empty;
        }
        public Status? GetSelectedStatus()
        {
            return ChangeStatusSelectedIndex switch
            {
                1 => Status.Accepted,
                2 => Status.Rejected,
                _ => null
            };
        }
        public Status? GetSelectedFilterStatus()
        {
            return StatusFilterSelectedIndex switch
            {
                1 => Status.Waiting,
                2 => Status.Accepted,
                3 => Status.Rejected,
                _ => null
            };
        }
        public bool ConfirmStatusChange()
        {
            if (SelectedRequest == null)
                return false;

            Status? newStatus = GetSelectedStatus();
            if (newStatus == null)
                return false;

            bool success = _apartmentAccessRequestService.UpdateStatus(
                SelectedRequest,
                newStatus.Value,
                Comment
            );

            if (!success)
                return false;

            var currentFilter = GetSelectedFilterStatus();
            LoadAccessRequests(currentFilter);
            ResetSelectionAndForm();
            return true;
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
