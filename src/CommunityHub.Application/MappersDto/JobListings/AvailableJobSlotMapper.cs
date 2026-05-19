using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.Services.Dtos.Services;

namespace CommunityHub.Application.MappersDto.JobListings
{
    public class AvailableJobSlotMapper : IMapper<AvailableJobSlot, AvailableJobSlotDto>
    {
        public AvailableJobSlotDto MapToDto(AvailableJobSlot slot)
        {
            if (slot == null) throw new ArgumentNullException(nameof(slot));

            return new AvailableJobSlotDto(
                slot.Date,
                slot.Price,
                slot.IsPriceChanged,
                slot.Time,
                slot.DurationHours);
        }

        public AvailableJobSlot MapToDomain(AvailableJobSlotDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new AvailableJobSlot(
                dto.Date,
                dto.Price,
                dto.IsPriceChanged,
                dto.Time,
                dto.DurationHours);
        }
    }
}
