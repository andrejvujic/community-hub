using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;
using CommunityHub.Application.Domain.Users.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodMembershipService
    {
        private readonly INeighborhoodMembershipRepository _neighborhoodMembershipRepository
            = Injector.CreateInstance<INeighborhoodMembershipRepository>();
        private readonly ICitizenRepository _citizenRepository
            = Injector.CreateInstance<ICitizenRepository>();
        private readonly CitizenMapper _citizenMapper
            = new CitizenMapper();
        private readonly NeighborhoodMapper _neighborhoodMapper
            = new NeighborhoodMapper();
        private readonly NeighborhoodMembershipMapper _neighborhoodMembershipMapper
            = new NeighborhoodMembershipMapper();

        public List<CitizenDto> GetCitizensByNeighborhood(long neighborhoodId)
            => _neighborhoodMembershipRepository.GetCitizensByNeighborhoodId(neighborhoodId)
                .Select(_citizenMapper.MapToDto)
                .ToList();

        public NeighborhoodMembershipDto Create(NeighborhoodDto neighborhoodDto)
        {
            var membership = new NeighborhoodMembership(
                0,
                DateTime.Now,
                _neighborhoodMapper.MapToDomain(neighborhoodDto)
            );
            var createdMembership = _neighborhoodMembershipRepository.Create(membership);
            return _neighborhoodMembershipMapper.MapToDto(createdMembership);
        }

        public void CreateMembershipAndUpdate(CitizenDto citizenDto, NeighborhoodDto neighborhoodDto)
        {
            var neighborhood = _neighborhoodMapper.MapToDomain(neighborhoodDto);
            var membership = new NeighborhoodMembership(
                0,
                DateTime.Now,
                neighborhood
            );

            var createdMembership = _neighborhoodMembershipRepository.Create(membership);
            _citizenRepository.UpdateMembership(citizenDto.Id, createdMembership.Id);
        }
    }
}
