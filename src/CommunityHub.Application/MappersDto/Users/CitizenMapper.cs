using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.MappersDto.Users
{
    public class CitizenMapper : IMapper<Citizen, CitizenDto>
    {
        private readonly AddressMapper _addressMapper = new AddressMapper();
        private readonly NeighborhoodMembershipMapper _neighborhoodMembershipMapper = new NeighborhoodMembershipMapper();
        
        public CitizenDto MapToDto(Citizen citizen)
        {
            if (citizen == null) throw new ArgumentNullException(nameof(citizen));

            return new CitizenDto(
                citizen.Id,
                citizen.Username,
                citizen.Password,
                citizen.Name,
                citizen.Surname,
                citizen.Role,
                _addressMapper.MapToDto(citizen.Address),
                citizen.Membership != null ? _neighborhoodMembershipMapper.MapToDto(citizen.Membership) : null
            );
        }

        public Citizen MapToDomain(CitizenDto citizenDto)
        {
            if (citizenDto == null) throw new ArgumentNullException(nameof(citizenDto));

            return new Citizen(
                citizenDto.Id,
                citizenDto.Username,
                citizenDto.Password,
                citizenDto.Name,
                citizenDto.Surname,
                citizenDto.Role,
                _addressMapper.MapToDomain(citizenDto.Address),
                citizenDto.Membership != null ? _neighborhoodMembershipMapper.MapToDomain(citizenDto.Membership) : null
            );
        }
    }
}
