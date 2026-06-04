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
        public StreetSection MapToEntity(StreetSectionDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new StreetSection(
                dto.StartNumber,
                dto.EndNumber,
                dto.Side,
                _streetMapper.MapToEntity(dto.Street),
                dto.Id
            );
        }
    }
}