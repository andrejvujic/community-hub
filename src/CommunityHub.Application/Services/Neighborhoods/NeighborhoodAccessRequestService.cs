using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.JobListingBookings;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodAccessRequestService
    {
        private readonly INeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository
            = Injector.CreateInstance<INeighborhoodAccessRequestRepository>();
        private readonly NeighborhoodAccessRequestMapper _neighborhoodAccessRequestMapper = new();
        private readonly CitizenMapper _citizenMapper = new();
        private readonly NeighborhoodMapper _neighborhoodMapper = new();

        public List<NeighborhoodAccessRequestDto> GetByCoordinatorAndStatus(long coordinatorId, NeighborhoodAccessRequestStatus? status)
            => _neighborhoodAccessRequestRepository.GetByCoordinatorIdAndStatus(coordinatorId, status)
                .Select(_neighborhoodAccessRequestMapper.MapToDto)
                .ToList();

        public List<NeighborhoodAccessRequestDto> GetByCitizenId(long citizenId, RequestOrder order = RequestOrder.Unspecified, bool filterByStatus = false)
        {
            var requests = _neighborhoodAccessRequestRepository.GetByCitizenId(citizenId, order)
                .Select(_neighborhoodAccessRequestMapper.MapToDto)
                .ToList();

            if (!filterByStatus)
                return requests;

            return requests
                .OrderBy(r => r.Status)
                .ThenBy(r => order == RequestOrder.CreationDateDescending ? -r.CreationDate.Ticks : r.CreationDate.Ticks)
                .ToList();
        }

        public bool HasPendingRequest(long citizenId, long neighborhoodId)
            => _neighborhoodAccessRequestRepository.HasPendingRequest(citizenId, neighborhoodId);

        public void Reject(NeighborhoodAccessRequestDto requestDto, string comment)
        {
            var request = _neighborhoodAccessRequestMapper.MapToDomain(requestDto);
            var rejected = request.Reject(comment);
            _neighborhoodAccessRequestRepository.Update(rejected);
        }

        public void Accept(NeighborhoodAccessRequestDto requestDto)
        {
            var request = _neighborhoodAccessRequestMapper.MapToDomain(requestDto);
            var accepted = request.Accept();
            _neighborhoodAccessRequestRepository.Update(accepted);
        }

        public void CreateAccessRequest(CitizenDto citizenDto, NeighborhoodDto neighborhoodDto)
        {
            var citizen = _citizenMapper.MapToDomain(citizenDto);
            var neighborhood = _neighborhoodMapper.MapToDomain(neighborhoodDto);

            var request = new NeighborhoodAccessRequest(
                citizen,
                neighborhood,
                NeighborhoodAccessRequestStatus.Waiting,
                DateTime.Now,
                null
            );

            _neighborhoodAccessRequestRepository.Create(request);
        }

        public void AutoRejectOtherRequests(CitizenDto citizenDto, long approvedNeighborhoodId)
        {
            var pending = _neighborhoodAccessRequestRepository.GetWaitingByCitizenId(citizenDto.Id);
            foreach (var r in pending.Where(r => r.Neighborhood.Id != approvedNeighborhoodId))
            {
                var updated = r.CopyWith(status: NeighborhoodAccessRequestStatus.Rejected, comment: "Automatski odbijen: građanin je pridružen drugom kvartu.");
                _neighborhoodAccessRequestRepository.Update(updated);
            }
        }

        public void Delete(long citizenId, long neighborhoodId)
            => _neighborhoodAccessRequestRepository.Delete(citizenId, neighborhoodId);
    }
}
