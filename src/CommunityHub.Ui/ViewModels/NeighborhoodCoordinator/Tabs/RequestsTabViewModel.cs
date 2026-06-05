using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Application.Services.Users;
using CommunityHub.Ui.Helpers.Commands;
using CommunityHub.Ui.Views.NeighborhoodCoordinator.Windows;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class RequestsTabViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodAccessRequestService _neighborhoodAccessRequestService = new();
        private readonly NeighborhoodMembershipService _neighborhoodMembershipService = new();
        private readonly CitizenService _citizenService = new();
        private long _coordinatorId;

        public ObservableCollection<NeighborhoodAccessRequestDto> Requests { get; } = new();

        public RelayCommand AcceptCommand { get; }
        public RelayCommand RejectCommand { get; }

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

        private NeighborhoodAccessRequestStatus? _selectedStatusFilter;
        public NeighborhoodAccessRequestStatus? SelectedStatusFilter
        {
            get => _selectedStatusFilter;
            set
            {
                if (_selectedStatusFilter == value) return;
                _selectedStatusFilter = value;
                OnPropertyChanged();
                LoadRequests(_selectedStatusFilter);
            }
        }

        public bool CanActOnRequest =>
            SelectedRequest != null && SelectedRequest.Status == NeighborhoodAccessRequestStatus.Waiting;

        public RequestsTabViewModel()
        {
            AcceptCommand = new RelayCommand(ExecuteAccept, () => CanActOnRequest);
            RejectCommand = new RelayCommand(ExecuteReject, () => CanActOnRequest);
        }

        public void Initialize(long coordinatorId)
        {
            _coordinatorId = coordinatorId;
            LoadRequests(null);
        }

        public void LoadRequests(NeighborhoodAccessRequestStatus? status)
        {
            Requests.Clear();
            var requests = _neighborhoodAccessRequestService.GetByCoordinatorAndStatus(_coordinatorId, status);
            foreach (var r in requests)
                Requests.Add(r);

            SelectedRequest = null;
        }

        private void UpdateLocalRequestList(NeighborhoodAccessRequestStatus status, string? comment = null)
        {
            if (SelectedRequest == null) return;
            int index = Requests.IndexOf(SelectedRequest);
            if (index < 0) return;

            var updated = new NeighborhoodAccessRequestDto(
                SelectedRequest.Citizen, SelectedRequest.Neighborhood, status, SelectedRequest.CreationDate, comment);
            Requests[index] = updated;
            SelectedRequest = updated;
            OnPropertyChanged(nameof(CanActOnRequest));
        }

        private void ExecuteAccept()
        {
            if (SelectedRequest == null) return;
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                         ?? System.Windows.Application.Current.MainWindow;

            var confirmMsg = $"Da li ste sigurni da želite da prihvatite zahtev korisnika {SelectedRequest.Citizen.Name} {SelectedRequest.Citizen.Surname} za pristup kvartu {SelectedRequest.Neighborhood.Name}?";
            var confirm = new ConfirmDialog("Potvrda prihvatanja", confirmMsg, window);
            confirm.ShowDialog();
            if (!confirm.Confirmed) return;

            _neighborhoodAccessRequestService.Accept(SelectedRequest);
            var membership = _neighborhoodMembershipService.Create(SelectedRequest.Neighborhood);
            _citizenService.UpdateMembership(SelectedRequest.Citizen.Id, membership.Id);

            UpdateLocalRequestList(NeighborhoodAccessRequestStatus.Accepted);
            var infoMsg = $"Korisnik {SelectedRequest.Citizen.Name} {SelectedRequest.Citizen.Surname} je postao novi član kvarta {SelectedRequest.Neighborhood.Name}.";
            new InfoDialog("Prihvaćen zahtev", infoMsg, window).ShowDialog();
        }

        private void ExecuteReject()
        {
            if (SelectedRequest == null) return;
            var window = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                         ?? System.Windows.Application.Current.MainWindow;

            var dialog = new RejectRequestWindow(SelectedRequest.Citizen.Name, SelectedRequest.Citizen.Surname, SelectedRequest.Neighborhood.Name) { Owner = window };
            if (dialog.ShowDialog() != true) return;

            _neighborhoodAccessRequestService.Reject(SelectedRequest, dialog.Comment);
            UpdateLocalRequestList(NeighborhoodAccessRequestStatus.Rejected, dialog.Comment);

            var infoMsg = $"Korisniku {SelectedRequest.Citizen.Name} {SelectedRequest.Citizen.Surname} je odbijen zahtev za pristup kvartu {SelectedRequest.Neighborhood.Name}.";
            new InfoDialog("Odbijen zahtev", infoMsg, window).ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}