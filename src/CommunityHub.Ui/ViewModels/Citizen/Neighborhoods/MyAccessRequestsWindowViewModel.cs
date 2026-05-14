using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace CommunityHub.Ui.ViewModels.Citizen.Neighborhoods
{
    public class MyAccessRequestsWindowViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public MyAccessRequestsWindowViewModel(long userId)
        {
            _userId = userId;
            RequestsView = CollectionViewSource.GetDefaultView(Requests);
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
}
