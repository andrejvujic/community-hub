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
        private readonly FacilityReservationMapper _reservationMapper = new();
        private readonly CityFacilityMapper _cityFacilityMapper = new();
        private readonly NeighborhoodMapper _neighborhoodMapper = new();
        private readonly AvailableFacilitySlotMapper _slotMapper = new();

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

        public (AvailableFacilitySlotDto? SlotInRange, List<AvailableFacilitySlotDto> SlotsOutsideRange)
            FindSlots(CityFacilityDto facilityDto, int durationHours, DateTime rangeStart, DateTime rangeEnd)
        {
            var cityFacility = _cityFacilityMapper.MapToEntity(facilityDto);
            var reservations = _reservationRepository.GetByFacilityId(cityFacility.Id);
            var (slotInRange, slotsOutside) = cityFacility.FindSlots(reservations, durationHours, rangeStart, rangeEnd);

            return (
                slotInRange is null ? null : _slotMapper.MapToDto(slotInRange),
                slotsOutside.Select(s => _slotMapper.MapToDto(s)).ToList()
            );
        }

        public FacilityReservationDto Book(CityFacilityDto facilityDto, NeighborhoodDto neighborhoodDto,
            AvailableFacilitySlotDto slotDto)
        {
            var facility = _cityFacilityMapper.MapToEntity(facilityDto);
            var neighborhood = _neighborhoodMapper.MapToEntity(neighborhoodDto);
            var slot = _slotMapper.MapToEntity(slotDto);
            var reservation = new FacilityReservation(facility, neighborhood, slot.Start, slot.End);
            var created = _reservationRepository.Create(reservation);
            return _reservationMapper.MapToDto(created);
        }
    }
}
