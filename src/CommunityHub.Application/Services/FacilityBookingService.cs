using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Neighborhoods.CityFacility;

namespace CommunityHub.Application.Services
{
    public class FacilityBookingService
    {
        private readonly FacilityReservationRepository _reservationRepository;

        public FacilityBookingService(FacilityReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public List<FacilityReservation> GetReservationsForNeighborhood(long neighborhoodId)
            => _reservationRepository.GetByNeighborhoodId(neighborhoodId);

        public (AvailableFacilitySlot? SlotInRange, List<AvailableFacilitySlot> SlotsOutsideRange)
            FindSlots(CityFacility facility, int durationHours, DateTime rangeStart, DateTime rangeEnd)
        {
            var tomorrow = DateTime.Today.AddDays(1);

            if (rangeStart.Date < tomorrow)
                rangeStart = tomorrow.AddHours(8);

            var reservations = _reservationRepository
                .GetByFacilityId(facility.Id)
                .OrderBy(r => r.StartDatetime)
                .ToList();

            var slotsInRange = FindAllSlotsInRange(reservations, rangeStart, rangeEnd, durationHours);

            if (slotsInRange.Count > 0)
                return (slotsInRange[0], new List<AvailableFacilitySlot>());

            var outsideStart = rangeEnd.Date.AddDays(1).AddHours(8);
            var outsideEnd = outsideStart.AddDays(90);
            var slotsOutside = FindAllSlotsInRange(reservations, outsideStart, outsideEnd, durationHours);

            return (null, slotsOutside);
        }

        private static List<AvailableFacilitySlot> FindAllSlotsInRange(
            List<FacilityReservation> reservations,
            DateTime rangeStart,
            DateTime rangeEnd,
            int durationHours,
            int maxSlots = 5)
        {
            var slots = new List<AvailableFacilitySlot>();
            var current = rangeStart;

            while (current < rangeEnd && slots.Count < maxSlots)
            {
                current = AdvanceToValidTime(current, durationHours);
                if (current >= rangeEnd) break;

                var overlapping = FindOverlappingReservation(reservations, current, durationHours);
                current = ProcessCandidate(slots, current, durationHours, overlapping);
            }

            return slots;
        }
        private static DateTime AdvanceToValidTime(DateTime current, int durationHours)
        {
            var dayStart = current.Date.AddHours(8);
            var dayEnd = current.Date.AddHours(22);

            if (current < dayStart)
                current = dayStart;

            if (current >= dayEnd)
                return current.Date.AddDays(1).AddHours(8);

            if (current.AddHours(durationHours) > dayEnd)
                return current.Date.AddDays(1).AddHours(8);

            return current;
        }

        private static FacilityReservation? FindOverlappingReservation(
            List<FacilityReservation> reservations,
            DateTime current,
            int durationHours)
        {
            return reservations
                .Where(r => r.StartDatetime < current.AddHours(durationHours)
                         && r.EndDatetime > current)
                .OrderBy(r => r.StartDatetime)
                .FirstOrDefault();
        }

        private static DateTime ProcessCandidate(
            List<AvailableFacilitySlot> slots,
            DateTime current,
            int durationHours,
            FacilityReservation? overlapping)
        {
            if (overlapping is null)
            {
                slots.Add(new AvailableFacilitySlot(current, current.AddHours(durationHours), false));
                return current.AddHours(durationHours);
            }

            return overlapping.EndDatetime;
        }

        public FacilityReservation BookSlot(CityFacility facility, Neighborhood neighborhood, AvailableFacilitySlot slot)
        {
            var reservation = new FacilityReservation(facility, neighborhood, slot.Start, slot.End);

            return _reservationRepository.Create(reservation);
        }
    }
}
