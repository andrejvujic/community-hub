using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities;
using CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities
{
    public class FacilityReservationService : BaseService<FacilityReservation, FacilityReservationDto>
    {
        private readonly IFacilityReservationRepository _reservationRepository = Injector.CreateInstance<IFacilityReservationRepository>();
        private readonly FacilityReservationMapper _reservationMapper = new();

        public FacilityReservationService() : base(mapper: new FacilityReservationMapper()) { }

        public List<FacilityReservationDto> GetByFacilityId(long facilityId)
        {
            var reservations = _reservationRepository.GetByFacilityId(facilityId);
            return MapToDto(reservations);
        }

        public List<FacilityReservationDto> GetByNeighborhoodId(long neighborhoodId)
        {
            var reservations = _reservationRepository.GetByNeighborhoodId(neighborhoodId);
            return MapToDto(reservations);
        }

        public DateTime? GetLastVisitDateForFacility(long facilityId)
        {
            var reservations = _reservationRepository.GetByFacilityId(facilityId);
            return reservations.OrderByDescending(r => r.EndDatetime).FirstOrDefault()?.EndDatetime;
        }
    }
}
