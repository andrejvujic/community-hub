using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

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

            var occupied = _bookedJobRepository.GetOccupiedSlots(listing.Id);
            var prices = listing.PriceList.OrderBy(p => p.ValidFrom).ToList();
            var today = DateOnly.FromDateTime(DateTime.Today);

            for (int i = 0; i < 365; i++)
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
            var workStart = new TimeOnly(8, 0);
            var workEnd = new TimeOnly(20, 0);
            var candidate = workStart;

            foreach (var slot in occupied)
            {
                if (candidate.AddHours(durationHours) <= slot.Start)
                    return candidate;
                if (slot.End > candidate)
                    candidate = slot.End;
            }

            return candidate.AddHours(durationHours) <= workEnd ? candidate : null;
        }
    }
}
