using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;
using CommunityHub.Application.Domain.Users.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.MappersDto.TrustRecords;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodAccessRequestService
    {
        private readonly INeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository
            = Injector.CreateInstance<INeighborhoodAccessRequestRepository>();
        private readonly INeighborhoodMembershipRepository _neighborhoodMembershipRepository
            = Injector.CreateInstance<INeighborhoodMembershipRepository>();
        private readonly ICitizenRepository _citizenRepository
            = Injector.CreateInstance<ICitizenRepository>();
        private readonly NeighborhoodAccessRequestMapper _neighborhoodAccessRequestMapper = new();
        private readonly CitizenMapper _citizenMapper = new();
        private readonly NeighborhoodMapper _neighborhoodMapper = new();

        public List<NeighborhoodAccessRequestDto> GetByCoordinatorAndStatus(long coordinatorId, Status? status)
            => _neighborhoodAccessRequestRepository.GetByCoordinatorIdAndStatus(coordinatorId, status)
                .Select(_neighborhoodAccessRequestMapper.MapToDto)
                .ToList();

        public bool HasPendingRequest(long citizenId, long neighborhoodId)
            => _neighborhoodAccessRequestRepository.HasPendingRequest(citizenId, neighborhoodId);

        public void Reject(NeighborhoodAccessRequestDto requestDto, string comment)
        {
            var request = _neighborhoodAccessRequestMapper.MapToDomain(requestDto);
            request.Reject(comment);
            _neighborhoodAccessRequestRepository.Update(request);
        }

        public void Accept(NeighborhoodAccessRequestDto requestDto)
        {
            var request = _neighborhoodAccessRequestMapper.MapToDomain(requestDto);
            request.Accept();
            _neighborhoodAccessRequestRepository.Update(request);
            var membership = new NeighborhoodMembership(
                0,
                DateTime.Now,
                request.Neighborhood
            );
            var createdMembership = _neighborhoodMembershipRepository.Create(membership);
            _citizenRepository.UpdateMembership(request.Citizen.Id, createdMembership.Id);
        }

        public void CreateAccessRequest(CitizenDto citizenDto, NeighborhoodDto neighborhoodDto)
        {
            var citizen = _citizenMapper.MapToDomain(citizenDto);
            var neighborhood = _neighborhoodMapper.MapToDomain(neighborhoodDto);

            var request = new NeighborhoodAccessRequest(
                citizen,
                neighborhood,
                Status.Waiting,
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
                var updated = r.CopyWith(status: Status.Rejected, comment: "Automatski odbijen: građanin je pridružen drugom kvartu.");
                _neighborhoodAccessRequestRepository.Update(updated);
            }
        }
    }
}
