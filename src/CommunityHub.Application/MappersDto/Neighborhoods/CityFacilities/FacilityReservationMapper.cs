using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities
{
    public class FacilityReservationMapper : IMapper<FacilityReservation, FacilityReservationDto>
    {
        private readonly CityFacilityMapper _facilityMapper = new();
        private readonly NeighborhoodMapper _neighborhoodMapper = new();

        public FacilityReservationDto MapToDto(FacilityReservation facilityReservation)
        {
            if (facilityReservation == null) throw new ArgumentNullException(nameof(facilityReservation));

            return new FacilityReservationDto(
                facilityReservation.Id,
                _facilityMapper.MapToDto(facilityReservation.Facility),
                _neighborhoodMapper.MapToDto(facilityReservation.Neighborhood),
                facilityReservation.StartDatetime,
                facilityReservation.EndDatetime
            );
        }

        public FacilityReservation MapToEntity(FacilityReservationDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new FacilityReservation(
                _facilityMapper.MapToEntity(dto.Facility),
                _neighborhoodMapper.MapToEntity(dto.Neighborhood),
                dto.StartDatetime,
                dto.EndDatetime,
                dto.Id
            );
        }
    }
}
