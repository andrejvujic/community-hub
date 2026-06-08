using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.RepositoryInterfaces.Users;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Users
{
    public class CitizenService
    {
        private readonly ICitizenRepository _citizenRepository = new CitizenRepository();
        private readonly IMapper<Citizen, CitizenDto> _citizenMapper = new CitizenMapper();

        public CitizenDto? GetCitizenById(long citizenId)
            => _citizenRepository.GetById(citizenId) is Citizen citizen
                ? _citizenMapper.MapToDto(citizen)
                : null;

        public List<CitizenDto> GetCitizensByNeighborhoodId(long neighborhoodId)
            => _citizenRepository.GetByNeighborhoodId(neighborhoodId)
                .Select(_citizenMapper.MapToDto)
                .ToList();

        public void UpdateMembership(long citizenId, long membershipId)
            => _citizenRepository.UpdateMembership(citizenId, membershipId);
    }
}
