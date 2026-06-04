using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.JobListingBookings;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.JobListings
{
    public class JobListing
    {
        public long Id { get; private set; }
        public string ServiceName { get; private set; }
        public string Description { get; private set; }
        public BillingType BillingType { get; private set; }
        public City City { get; private set; }
        public Country Country { get; private set; }
        public ServiceProvider Provider { get; private set; }

        private readonly List<PriceEntry> _priceList = new List<PriceEntry>();
        private readonly List<Image> _workSamples = new List<Image>();

        public IReadOnlyList<PriceEntry> PriceList => _priceList.AsReadOnly();
        public IReadOnlyList<Image> WorkSamples => _workSamples.AsReadOnly();

        public PriceEntry? CurrentPriceEntry =>
            _priceList.FirstOrDefault(e => e.IsActiveOn(DateOnly.FromDateTime(DateTime.Today)));

        public JobListing(
            string serviceName,
            string description,
            BillingType billingType,
            City city,
            Country country,
            ServiceProvider provider,
            long id = 0,
            IEnumerable<PriceEntry>? priceList = null,
            IEnumerable<Image>? workSamples = null
        )
        {
            if (string.IsNullOrWhiteSpace(serviceName))
                throw new ArgumentException("Naziv usluge ne sme biti prazan.", nameof(serviceName));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Opis usluge ne sme biti prazan.", nameof(description));

            Id = id;
            ServiceName = serviceName;
            Description = description;
            BillingType = billingType;
            City = city;
            Country = country;
            Provider = provider;

            if (priceList is not null) _priceList.AddRange(priceList);
            if (workSamples is not null) _workSamples.AddRange(workSamples);
        }

        public void AddPriceEntry(PriceEntry entry)
        {
            ArgumentNullException.ThrowIfNull(entry);
            _priceList.Add(entry);
        }

        public void AddWorkSample(Image image)
        {
            ArgumentNullException.ThrowIfNull(image);
            _workSamples.Add(image);
        }

        public void SetWorkSamples(IEnumerable<Image> images)
        {
            ArgumentNullException.ThrowIfNull(images);
            _workSamples.Clear();
            _workSamples.AddRange(images);
        }

        public AvailableJobSlot? FindFirstAvailableSlot(
            List<BookedJob> bookedJobs,
            int? durationHours)
        {
            if (BillingType == BillingType.PerHour && durationHours is null) return null;

            if (BillingType == BillingType.PerHour && durationHours > 14)
                throw new ArgumentException("Trajanje usluge ne može biti duže od 14 sati.");

            var prices = _priceList.OrderBy(p => p.ValidFrom).ToList();
            var today = DateOnly.FromDateTime(DateTime.Today);

            for (int i = 1; i < 60; i++)
            {
                var candidate = today.AddDays(i);
                var slot = FindSlotOnDay(candidate, durationHours, bookedJobs, prices);
                if (slot is not null) return slot;
            }

            return null;
        }

        private AvailableJobSlot? FindSlotOnDay(
            DateOnly candidate,
            int? durationHours,
            List<BookedJob> bookedJobs,
            List<PriceEntry> prices)
        {
            var priceEntry = prices.FirstOrDefault(p => p.IsActiveOn(candidate));
            if (priceEntry is null) return null;

            bool isPriceChanged = CurrentPriceEntry is null || priceEntry.Id != CurrentPriceEntry.Id;

            return BillingType == BillingType.PerJob
               ? FindPerJobSlot(candidate, bookedJobs, priceEntry.Amount, isPriceChanged)
               : FindPerHourSlot(candidate, durationHours!.Value, bookedJobs, priceEntry.Amount, isPriceChanged);
        }

        private static AvailableJobSlot? FindPerJobSlot(
            DateOnly candidate,
            List<BookedJob> bookedJobs,
            decimal price,
            bool isPriceChanged)
        {
            bool isOccupied = bookedJobs.Any(b => b.ScheduledDate == candidate);
            if (isOccupied) return null;

            return new AvailableJobSlot(candidate, price, isPriceChanged);
        }

        private static AvailableJobSlot? FindPerHourSlot(
            DateOnly candidate,
            int durationHours,
            List<BookedJob> bookedJobs,
            decimal price,
            bool isPriceChanged)
        {

            var occupiedOnDay = bookedJobs
                .Where(o => o.ScheduledDate == candidate && o.ScheduledTime.HasValue)
                .Select(o => (
                    Start: o.ScheduledTime!.Value,
                    End: o.ScheduledTime.Value.AddHours(o.DurationHours ?? 1)))
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
            const int workEndMinutes = 22 * 60;

            if (durationHours * 60 > workEndMinutes - workStartMinutes) return null;

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

        public JobListing CopyWith(
            long? id = null,
            string? serviceName = null,
            string? description = null,
            BillingType? billingType = null,
            City? city = null,
            Country? country = null,
            ServiceProvider? provider = null,
            IEnumerable<PriceEntry>? priceList = null,
            IEnumerable<Image>? workSamples = null
        )
        {
            return new JobListing(
                serviceName ?? ServiceName,
                description ?? Description,
                billingType ?? BillingType,
                city ?? City,
                country ?? Country,
                provider ?? Provider,
                id ?? Id,
                priceList ?? _priceList,
                workSamples ?? _workSamples
            );
        }
    }
}