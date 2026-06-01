using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.Domain.Neighborhoods.CityFacilities
{
    public class CityFacility
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public City City { get; private set; }
        public Country Country { get; private set; }

        public (AvailableFacilitySlot? SlotInRange, List<AvailableFacilitySlot> SlotsOutsideRange)
            FindSlots(
                List<FacilityReservation> existingReservations,
                int durationHours,
                DateTime rangeStart,
                DateTime rangeEnd)
        {
            var ordered = existingReservations.OrderBy(r => r.StartDatetime).ToList();

            var slotInRange = FindAllSlotsInRange(ordered, rangeStart, rangeEnd, durationHours, maxSlots: 1)
                .FirstOrDefault();

            if (slotInRange is not null)
                return (slotInRange, new List<AvailableFacilitySlot>());

            var outsideStart = rangeEnd.Date.AddDays(1).AddHours(8);
            var slotsOutside = FindAllSlotsInRange(ordered, outsideStart, outsideStart.AddDays(90), durationHours);
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

                var overlapping = reservations
                    .Where(r => r.StartDatetime < current.AddHours(durationHours) && r.EndDatetime > current)
                    .OrderBy(r => r.StartDatetime)
                    .FirstOrDefault();

                if (overlapping is null)
                {
                    slots.Add(new AvailableFacilitySlot(current, current.AddHours(durationHours), false));
                    current = current.AddHours(durationHours);
                }
                else
                {
                    current = overlapping.EndDatetime;
                }
            }
            return slots;
        }

        private static DateTime AdvanceToValidTime(DateTime current, int durationHours)
        {
            var dayStart = current.Date.AddHours(8);
            var dayEnd = current.Date.AddHours(22);

            if (current < dayStart) current = dayStart;
            if (current >= dayEnd || current.AddHours(durationHours) > dayEnd)
                return current.Date.AddDays(1).AddHours(8);

            return current;
        }

        public CityFacility(
            string name,
            City city,
            Country country,
            long id = 0
        )
        {
            Id = id;
            Name = name;
            City = city;
            Country = country;
        }

        public CityFacility CopyWith(
            long? id = null,
            string? name = null,
            City? city = null,
            Country? country = null
        )
        {
            return new CityFacility(
                name ?? Name,
                city ?? City,
                country ?? Country,
                id ?? Id
            );
        }
    }
}