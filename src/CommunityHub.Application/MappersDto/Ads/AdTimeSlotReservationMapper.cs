using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Application.MappersDto.Ads
{
    public class AdTimeSlotReservationMapper : IMapper<AdTimeSlotReservation, AdTimeSlotReservationDto>
    {
        private readonly AdMapper _adMapper = new();
        public AdTimeSlotReservationDto MapToDto(AdTimeSlotReservation reservation)
        {
            if (reservation == null) throw new ArgumentNullException(nameof(reservation));

            return new AdTimeSlotReservationDto(
                reservation.Id,
                reservation.OfferTimeSlotId,
                reservation.RequestTimeSlotId,
                reservation.OfferAd != null ? _adMapper.MapToDto(reservation.OfferAd) : null,
                reservation.RequestAd != null ? _adMapper.MapToDto(reservation.RequestAd) : null
            );
        }
        public AdTimeSlotReservation MapToEntity(AdTimeSlotReservationDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new AdTimeSlotReservation(
                dto.OfferTimeSlotId,
                dto.RequestTimeSlotId,
                dto.OfferAd != null ? _adMapper.MapToEntity(dto.OfferAd) : null,
                dto.RequestAd != null ? _adMapper.MapToEntity(dto.RequestAd) : null,
                dto.Id
            );
        }
    }
}