using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Application.MappersDto.Ads
{
    public class AdTimeSlotMapper : IMapper<AdTimeSlot, AdTimeSlotDto>
    {
        private readonly AdMapper _adMapper = new();
        private readonly AdTimeSlotReservationMapper _adTimeSlotReservationMapper = new();
        public AdTimeSlotDto MapToDto(AdTimeSlot timeSlot)
        {
            if (timeSlot == null) throw new ArgumentNullException(nameof(timeSlot));

            return new AdTimeSlotDto(
                timeSlot.Id,
                timeSlot.AdId,
                timeSlot.Ad != null ? _adMapper.MapToDto(timeSlot.Ad) : null,
                timeSlot.StartTime,
                timeSlot.EndTime,
                timeSlot.Reservation != null ? _adTimeSlotReservationMapper.MapToDto(timeSlot.Reservation) : null
            );
        }
        public AdTimeSlot MapToDomain(AdTimeSlotDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new AdTimeSlot(
                dto.AdId,
                dto.StartTime,
                dto.EndTime,
                dto.Ad != null ? _adMapper.MapToDomain(dto.Ad) : null,
                dto.Reservation != null ? _adTimeSlotReservationMapper.MapToDomain(dto.Reservation) : null,
                dto.Id
            );
        }
    }
}