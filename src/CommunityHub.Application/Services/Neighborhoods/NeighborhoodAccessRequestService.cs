using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;
using CommunityHub.Application.Domain.Users.RepositoryInterfaces;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.Services.Dtos.Neighborhoods;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodAccessRequestService
    {
        private readonly INeighborhoodAccessRequestRepository _neighborhoodAccessRequestRepository
            = Injector.CreateInstance<INeighborhoodAccessRequestRepository> ();
        private readonly INeighborhoodMembershipRepository _neighborhoodMembershipRepository
            = Injector.CreateInstance<INeighborhoodMembershipRepository> ();
        private readonly ICitizenRepository _citizenRepository
            = Injector.CreateInstance<ICitizenRepository> ();
        private readonly IMapper<NeighborhoodAccessRequest, NeighborhoodAccessRequestDto> _neighborhoodAccessRequestMapper
            = Injector.CreateInstance<IMapper<NeighborhoodAccessRequest, NeighborhoodAccessRequestDto>>();


        public List<NeighborhoodAccessRequestDto> GetByCoordinatorAndStatus(long coordinatorId, Status? status)
            => _neighborhoodAccessRequestRepository.GetByCoordinatorIdAndStatus(coordinatorId, status)
                .Select(_neighborhoodAccessRequestMapper.MapToDto)
                .ToList();

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
    }
}
