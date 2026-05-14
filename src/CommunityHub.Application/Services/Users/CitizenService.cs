using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Domain.Users.RepositoryInterfaces;
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
    }
}
