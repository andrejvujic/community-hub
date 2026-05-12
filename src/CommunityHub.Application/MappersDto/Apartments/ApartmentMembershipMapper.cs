using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.MappersDto.Users;

namespace CommunityHub.Application.MappersDto.Apartments
{
    public class ApartmentMembershipMapper : IMapper<ApartmentMembership, ApartmentMembershipDto>
    {
        private readonly ApartmentMapper _apartmentMapper = new();
        private readonly UserMapper _userMapper = new();
        public ApartmentMembershipDto MapToDto(ApartmentMembership apartmentMembership)
        {
            if (apartmentMembership == null) throw new ArgumentNullException(nameof(apartmentMembership));

            return new ApartmentMembershipDto(
                apartmentMembership.Id,
                _userMapper.MapToDto(apartmentMembership.Resident),
                _apartmentMapper.MapToDto(apartmentMembership.Apartment),
                apartmentMembership.AcceptanceDate
            );
        }
        public ApartmentMembership MapToDomain(ApartmentMembershipDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new ApartmentMembership(
                _userMapper.MapToDomain(dto.Resident),
                _apartmentMapper.MapToDomain(dto.Apartment),
                dto.AcceptanceDate,
                dto.Id
            );
        }
    }
}