using CommunityHub.Application.Database.Repositories.JobListingBookings;
using CommunityHub.Application.Database.Repositories.JobListings;
using CommunityHub.Application.Domain.JobListingBookings;
using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.Domain.Neighborhoods;

namespace CommunityHub.Application.Services
{
    public class JobBookingService
    {
        private readonly JobListingRepository _jobListingRepository;
        private readonly BookedJobRepository _bookedJobRepository;

        public JobBookingService(
            JobListingRepository jobListingRepository,
            BookedJobRepository bookedJobRepository)
        {
            _jobListingRepository = jobListingRepository;
            _bookedJobRepository = bookedJobRepository;
        }

        public List<JobListing> GetJobListingsForLocation(long cityId, long countryId)
            => _jobListingRepository.GetByCityAndCountry(cityId, countryId);

        public List<BookedJob> GetBookedJobListingsForNeighborhood(long neighborhoodId)
            => _bookedJobRepository.GetByNeighborhoodId(neighborhoodId);

        public AvailableJobSlot? FindFirstAvailableSlot(JobListing listing, int? durationHours)
        {
            if (listing.BillingType == BillingType.PerHour && durationHours is null)
                return null;

            if (listing.BillingType == BillingType.PerHour && durationHours > 12)
                return null;

            var occupied = _bookedJobRepository.GetOccupiedSlots(listing.Id);
            var prices = listing.PriceList.OrderBy(p => p.ValidFrom).ToList();
            var today = DateOnly.FromDateTime(DateTime.Today);

            for (int i = 1; i < 365; i++)
            {
                var candidate = today.AddDays(i);
                var slot = FindSlotOnDay(listing, candidate, durationHours, occupied, prices);
                if (slot is not null) return slot;
            }

            return null;
        }

        private AvailableJobSlot? FindSlotOnDay(
            JobListing listing,
            DateOnly candidate,
            int? durationHours,
            List<(DateOnly Date, TimeOnly? Time, int? DurationHours)> occupied,
            List<PriceEntry> prices)
        {
            var priceEntry = prices.FirstOrDefault(p => p.IsActiveOn(candidate));
            if (priceEntry is null) return null;

            bool isPriceChanged = listing.CurrentPriceEntry is null
                || priceEntry.Id != listing.CurrentPriceEntry.Id;

            return listing.BillingType == BillingType.PerJob
                ? FindPerJobSlot(candidate, occupied, priceEntry.Amount, isPriceChanged)
                : FindPerHourSlot(candidate, durationHours!.Value, occupied, priceEntry.Amount, isPriceChanged);
        }

        private static AvailableJobSlot? FindPerJobSlot(
            DateOnly candidate,
            List<(DateOnly Date, TimeOnly? Time, int? DurationHours)> occupied,
            decimal price,
            bool isPriceChanged)
        {
            bool isOccupied = occupied.Any(o => o.Date == candidate);
            if (isOccupied) return null;

            return new AvailableJobSlot(candidate, price, isPriceChanged);
        }

        private static AvailableJobSlot? FindPerHourSlot(
            DateOnly candidate,
            int durationHours,
            List<(DateOnly Date, TimeOnly? Time, int? DurationHours)> occupied,
            decimal price,
            bool isPriceChanged)
        {
            var occupiedOnDay = occupied
                .Where(o => o.Date == candidate && o.Time.HasValue)
                .Select(o => (
                    Start: o.Time!.Value,
                    End: o.Time.Value.AddHours(o.DurationHours ?? 1)))
                .OrderBy(o => o.Start)
                .ToList();

            var freeTime = FindFreeHourSlot(occupiedOnDay, durationHours);
            if (freeTime is null) return null;

            return new AvailableJobSlot(candidate, price, isPriceChanged, time: freeTime, durationHours: durationHours);
        }

        private static TimeOnly? FindFreeHourSlot(
            List<(TimeOnly Start, TimeOnly End)> occupied,
            int durationHours)
        {
            const int workStartMinutes = 8 * 60;
            const int workEndMinutes = 20 * 60;

            if (durationHours * 60 > workEndMinutes - workStartMinutes)
                return null;

            int candidateMinutes = workStartMinutes;

            foreach (var slot in occupied)
            {
                int slotStartMinutes = slot.Start.Hour * 60 + slot.Start.Minute;
                int slotEndMinutes = slot.End.Hour * 60 + slot.End.Minute;

                if (candidateMinutes + durationHours * 60 <= slotStartMinutes)
                    return new TimeOnly(candidateMinutes / 60, candidateMinutes % 60);

                if (slotEndMinutes > candidateMinutes)
                    candidateMinutes = slotEndMinutes;
            }

            if (candidateMinutes + durationHours * 60 <= workEndMinutes)
                return new TimeOnly(candidateMinutes / 60, candidateMinutes % 60);

            return null;
        }

        public BookedJob BookService(JobListing listing, Neighborhood neighborhood, AvailableJobSlot slot)
        {
            var job = new BookedJob(listing, neighborhood, slot.Date, slot.Price, scheduledTime: slot.Time, durationHours: slot.DurationHours);

            return _bookedJobRepository.Create(job);
        }
    }
}
