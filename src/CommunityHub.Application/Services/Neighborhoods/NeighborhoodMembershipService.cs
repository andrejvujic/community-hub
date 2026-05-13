using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.RepositoryInterfaces;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class NeighborhoodMembershipService
    {
        private readonly INeighborhoodMembershipRepository _neighborhoodMembershipRepository
            = Injector.CreateInstance<INeighborhoodMembershipRepository>();
        private readonly IMapper<Citizen, CitizenDto> _citizenMapper
            = Injector.CreateInstance<IMapper<Citizen, CitizenDto>>();
        private readonly IMapper<Neighborhood, NeighborhoodDto> _neighborhoodMapper
            = Injector.CreateInstance<IMapper<Neighborhood, NeighborhoodDto>>();
        private readonly IMapper<NeighborhoodMembership, NeighborhoodMembershipDto> _neighborhoodMembershipMapper
            = Injector.CreateInstance<IMapper<NeighborhoodMembership, NeighborhoodMembershipDto>>();

        public List<CitizenDto> GetCitizensByNeighborhood(long neighborhoodId)
            => _neighborhoodMembershipRepository.GetCitizensByNeighborhoodId(neighborhoodId).Select(_citizenMapper.MapToDto).ToList();
        
        public NeighborhoodMembershipDto Register(NeighborhoodDto neighborhoodDto)
        {
            var membership = new NeighborhoodMembership(
                0,
                DateTime.Now,
                _neighborhoodMapper.MapToDomain(neighborhoodDto)
            );
            var createdMembership = _neighborhoodMembershipRepository.Create(membership);
            return _neighborhoodMembershipMapper.MapToDto(createdMembership);
        }
    }
}
