using CommunityHub.Application.Domain.Users.RepositoryInterfaces;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.MappersDto.Users;

namespace CommunityHub.Application.Services.Users
{
    public class CitizenService
    {
        private readonly ICitizenRepository _citizenRepository;
        private readonly CitizenMapper _mapper = new();

        public CitizenService(ICitizenRepository citizenRepository)
        {
            _citizenRepository = citizenRepository;
        }

        public CitizenDto? GetCitizenById(long citizenId)
        {
            var citizen = _citizenRepository.GetById(citizenId);
            return citizen != null ? _mapper.MapToDto(citizen) : null;
        }
    }
}
