using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities
{
    public class AvailableFacilitySlotMapper : IMapper<AvailableFacilitySlot, AvailableFacilitySlotDto>
    {
        public AvailableFacilitySlotDto MapToDto(AvailableFacilitySlot slot)
        {
            if (slot == null) throw new ArgumentNullException(nameof(slot));

            return new AvailableFacilitySlotDto(slot.Start, slot.End, slot.IsOutsideRange);
        }

        public AvailableFacilitySlot MapToEntity(AvailableFacilitySlotDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new AvailableFacilitySlot(dto.Start, dto.End, dto.IsOutsideRange);
        }
    }
}