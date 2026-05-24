using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Ui.Helpers.Navigation;
using CommunityHub.Ui.Views.Resident.Apartments;
using System.Collections.ObjectModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Apartments
{
    public class ApartmentAccessRequestsViewModel : BaseViewModel
    {
        private readonly ApartmentAccessRequestService _accessRequestService = new();
        public long UserId { get; set; }

        public ObservableCollection<ApartmentAccessRequestDto> Requests { get; private set; } = new();
        public bool HasRequests { get => Requests.Count > 0; }

        public ApartmentAccessRequestsViewModel(long userId, MainNavigationConsumerPage page) : base(page)
        {
            UserId = userId;
        }

        public void OpenRequestDetailsPage(ApartmentAccessRequestDto request)
            => Push(
                new ApartmentAccessRequestDetailsPage(request),
                onPop: _ => UpdateRequests()
            );

        public void UpdateRequests()
        {
            var requests = _accessRequestService.GetByApplicant(
                applicantId: UserId,
                status: SelectedStatus,
                order: SelectedOrder
            );

            Requests.Clear();
            foreach (var request in requests)
                Requests.Add(request);

            OnPropertyChanged(nameof(HasRequests));
        }


        private RequestOrder _selectedOrder = RequestOrder.Unspecified;
        public RequestOrder SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder == value) return;
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        private Status? _selectedStatus;
        public Status? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus == value) return;
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }
        public List<StatusOption> StatusOptions { get; } =
            [
                new("Svi", value: null),
                new("Prihvaćen", value: Status.Accepted),
                new("Odbijen", value: Status.Rejected),
                new("Čeka odobrenje", value: Status.Waiting)
            ];

    }
}
