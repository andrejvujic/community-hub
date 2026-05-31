using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities
{
    public class FacilityReservationService : BaseService<FacilityReservation, FacilityReservationDto>
    {
        private readonly IFacilityReservationRepository _reservationRepository
            = Injector.CreateInstance<IFacilityReservationRepository>();
        private readonly ICityFacilityRepository _cityFacilityRepository
            = Injector.CreateInstance<ICityFacilityRepository>();
        private readonly INeighborhoodRepository _neighborhoodRepository
            = Injector.CreateInstance<INeighborhoodRepository>();
        private readonly FacilityReservationMapper _reservationMapper = new();
        private readonly CityFacilityMapper _cityFacilityMapper = new();
        private readonly NeighborhoodMapper _neighborhoodMapper = new();

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

        public (AvailableFacilitySlot? SlotInRange, List<AvailableFacilitySlot> SlotsOutsideRange)
            FindSlots(CityFacilityDto facilityDto, int durationHours, DateTime rangeStart, DateTime rangeEnd)
        {
            var cityFacility = _cityFacilityMapper.MapToEntity(facilityDto);
            var reservations = _reservationRepository.GetByFacilityId(facilityDto.Id);
            return cityFacility.FindSlots(reservations, durationHours, rangeStart, rangeEnd);
        }

        public FacilityReservationDto Book(CityFacilityDto facilityDto, NeighborhoodDto neighborhoodDto,
            AvailableFacilitySlot slot)
        {
            var facility = _cityFacilityMapper.MapToEntity(facilityDto);
            var neighborhood = _neighborhoodMapper.MapToEntity(neighborhoodDto);
            var reservation = new FacilityReservation(facility, neighborhood, slot.Start, slot.End);
            var created = _reservationRepository.Create(reservation);
            return _reservationMapper.MapToDto(created);
        }
    }
}
