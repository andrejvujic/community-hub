using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Users;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class RequestsTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodAccessRequestService _neighborhoodAccessRequestService = new();
        private readonly NeighborhoodMembershipService _neighborhoodMembershipService = new();
        private readonly CitizenService _citizenService = new();
        private readonly long _coordinatorId;

        public ObservableCollection<NeighborhoodAccessRequestDto> Requests { get; } = new();

        private NeighborhoodAccessRequestDto? _selectedRequest;
        public NeighborhoodAccessRequestDto? SelectedRequest
        {
            get => _selectedRequest;
            set
            {
                if (_selectedRequest == value) return;
                _selectedRequest = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanActOnRequest));
            }
        }

        public bool CanActOnRequest =>
            SelectedRequest != null && SelectedRequest.Status == Status.Waiting;

        public RequestsTabViewModel(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
        }

        public void LoadRequests(Status? status)
        {
            Requests.Clear();
            var requests = _neighborhoodAccessRequestService.GetByCoordinatorAndStatus(_coordinatorId, status);
            foreach (var r in requests)
                Requests.Add(r);

            SelectedRequest = null;
        }

        public bool Reject(string comment)
        {
            if (SelectedRequest == null || SelectedRequest.Status != Status.Waiting)
                return false;

            _neighborhoodAccessRequestService.Reject(SelectedRequest, comment);

            int index = Requests.IndexOf(SelectedRequest);
            if (index >= 0)
            {
                var updatedRequest = new NeighborhoodAccessRequestDto(
                    SelectedRequest.Citizen,
                    SelectedRequest.Neighborhood,
                    Status.Rejected,
                    SelectedRequest.CreationDate,
                    comment
                );
                Requests[index] = updatedRequest;
                SelectedRequest = updatedRequest;
            }

            OnPropertyChanged(nameof(CanActOnRequest));
            return true;
        }

        public bool Accept()
        {
            if (SelectedRequest == null || SelectedRequest.Status != Status.Waiting)
                return false;

            _neighborhoodAccessRequestService.Accept(SelectedRequest);
            var newMembership = _neighborhoodMembershipService.Create(SelectedRequest.Neighborhood);
            _citizenService.UpdateMembership(SelectedRequest.Citizen.Id, newMembership.Id);

            int index = Requests.IndexOf(SelectedRequest);
            if (index >= 0)
            {
                var updatedRequest = new NeighborhoodAccessRequestDto(
                    SelectedRequest.Citizen,
                    SelectedRequest.Neighborhood,
                    Status.Accepted,
                    SelectedRequest.CreationDate
                );
                Requests[index] = updatedRequest;
                SelectedRequest = updatedRequest;
            }

            OnPropertyChanged(nameof(CanActOnRequest));
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}