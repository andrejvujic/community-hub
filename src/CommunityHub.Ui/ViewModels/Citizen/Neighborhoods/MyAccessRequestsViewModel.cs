using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods;
using CommunityHub.Ui.Helpers.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods
{
    public class MyAccessRequestsViewModel : INotifyPropertyChanged
    {
        private readonly NeighborhoodAccessRequestService _accessRequestService = new();
        private readonly long _userId;

        private RequestOrder _selectedOrder = RequestOrder.CreationDateDescending;
        private bool _filterByStatus = false;

        public ObservableCollection<NeighborhoodAccessRequestDto> Requests { get; } = new();

        public RequestOrder SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder == value) return;
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
                LoadRequests();
            }
        }

        public bool FilterByStatus
        {
            get => _filterByStatus;
            set
            {
                if (_filterByStatus == value) return;
                _filterByStatus = value;
                OnPropertyChanged(nameof(FilterByStatus));
                LoadRequests();
                ApplyGrouping();
            }
        }

        public ICollectionView RequestsView { get; }

        public NeighborhoodAccessRequestDto? SelectedRequest
        {
            get => _selectedRequest;
            set
            {
                if (_selectedRequest == value) return;
                _selectedRequest = value;
                OnPropertyChanged(nameof(SelectedRequest));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? CloseRequested;
        public event EventHandler<DeleteRequestedEventArgs>? DeleteRequested;

        public ICommand CloseCommand { get; }
        public ICommand ToggleSortCommand { get; }
        public ICommand ToggleFilterCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DeleteSelectedCommand { get; }

        private NeighborhoodAccessRequestDto? _selectedRequest;

        public MyAccessRequestsViewModel(long userId)
        {
            _userId = userId;
            RequestsView = CollectionViewSource.GetDefaultView(Requests);

            CloseCommand = new RelayCommand<object>(_ => RequestClose());
            ToggleSortCommand = new RelayCommand<object>(_ => ToggleSort());
            ToggleFilterCommand = new RelayCommand<object>(_ => ToggleFilter());
            DeleteCommand = new RelayCommand<NeighborhoodAccessRequestDto>(request => RequestDelete(request));
            DeleteSelectedCommand = new RelayCommand<object>(_ => DeleteSelected(), _ => CanDeleteSelected());

            LoadRequests();
        }

        public void LoadRequests()
        {
            var requests = _accessRequestService.GetByCitizenId(_userId, _selectedOrder, _filterByStatus);
            Requests.Clear();
            foreach (var request in requests)
            {
                Requests.Add(request);
            }
            ApplyGrouping();
        }

        public void DeleteRequest(NeighborhoodAccessRequestDto request)
        {
            _accessRequestService.Delete(request.Citizen.Id, request.Neighborhood.Id);
            Requests.Remove(request);
        }

        public void ToggleSort()
        {
            SelectedOrder = SelectedOrder == RequestOrder.CreationDateDescending
                ? RequestOrder.CreationDateAscending
                : RequestOrder.CreationDateDescending;
        }

        public void ToggleFilter()
        {
            FilterByStatus = !FilterByStatus;
        }

        public void RequestClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        public void RequestDelete(NeighborhoodAccessRequestDto request)
        {
            DeleteRequested?.Invoke(this, new DeleteRequestedEventArgs(request));
        }

        public void DeleteSelected()
        {
            if (_selectedRequest != null)
            {
                RequestDelete(_selectedRequest);
            }
        }

        public bool CanDeleteSelected()
        {
            return _selectedRequest != null && IsRequestDeletable(_selectedRequest);
        }

        private bool IsRequestDeletable(NeighborhoodAccessRequestDto request)
        {
            return request.Status == CommunityHub.Application.Domain.Neighborhoods.NeighborhoodAccessRequestStatus.Waiting;
        }

        private void ApplyGrouping()
        {
            if (RequestsView == null) return;

            RequestsView.GroupDescriptions.Clear();
            if (FilterByStatus)
            {
                RequestsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(NeighborhoodAccessRequestDto.Status)));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class DeleteRequestedEventArgs : EventArgs
    {
        public NeighborhoodAccessRequestDto Request { get; }

        public DeleteRequestedEventArgs(NeighborhoodAccessRequestDto request)
        {
            Request = request;
        }
    }
}
