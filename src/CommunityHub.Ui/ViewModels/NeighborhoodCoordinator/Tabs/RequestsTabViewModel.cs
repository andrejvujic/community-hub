using System.Collections.ObjectModel;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Neighborhoods;

namespace CommunityHub.Ui.ViewModels.NeighborhoodCoordinator.Tabs
{
    public class RequestsTabViewModel
    {
        private readonly NeighborhoodAccessRequestService _neighborhoodAccessRequestService= new();
        private readonly long _coordinatorId;

        public ObservableCollection<NeighborhoodAccessRequestDto> Requests { get; } = new();

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
        }

        public void Reject(NeighborhoodAccessRequestDto dto, string comment)
            => _neighborhoodAccessRequestService.Reject(dto, comment);

        public void Accept(NeighborhoodAccessRequestDto dto)
            => _neighborhoodAccessRequestService.Accept(dto);
    }
}
