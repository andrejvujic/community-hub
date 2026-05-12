using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.MappersDto.Addresses
{
    public class StreetSectionMapper : IMapper<StreetSection, StreetSectionDto>
    {
        private readonly StreetMapper _streetMapper = new();

        public StreetSectionDto MapToDto(StreetSection streetSection)
        {
            if (streetSection == null) throw new ArgumentNullException(nameof(streetSection));
            return new StreetSectionDto(
                streetSection.Id,
                streetSection.StartNumber,
                streetSection.EndNumber,
                streetSection.Side,
                _streetMapper.MapToDto(streetSection.Street)
            );
        }

        public StreetSection MapToDomain(StreetSectionDto streetSectionDto)
        {
            if (streetSectionDto == null) throw new ArgumentNullException(nameof(streetSectionDto));
            return new StreetSection(
                streetSectionDto.StartNumber,
                streetSectionDto.EndNumber,
                streetSectionDto.Side,
                _streetMapper.MapToDomain(streetSectionDto.Street),
                streetSectionDto.Id
            );
        }
    }
}