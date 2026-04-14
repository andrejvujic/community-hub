namespace CommunityHub.Application.Domain
{
    public class BookedJob
    {
        public long Id { get; private set; }
        public JobListing Listing { get; private set; }
        public Neighborhood Neighborhood { get; private set; }
        public DateOnly ScheduledDate { get; private set; }

        public TimeOnly? ScheduledTime { get; private set; }
        public int? DurationHours { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal TotalPrice => DurationHours.HasValue
            ? UnitPrice * DurationHours.Value
            : UnitPrice;

        public bool IsHourlyBilling => Listing.BillingType == BillingType.PerHour;

        public bool IsSubstitution { get; private set; }

        public ServiceProvider? SubstitutingProvider { get; private set; }

        public BookedJob(
            JobListing listing,
            Neighborhood neighborhood,
            DateOnly scheduledDate,
            decimal unitPrice,
            long id = 0,
            TimeOnly? scheduledTime = null,
            int? durationHours = null,
            bool isSubstitution = false,
            ServiceProvider? substitutingProvider = null
        )
        {
            if (listing.BillingType == BillingType.PerHour && durationHours == null)
                throw new ArgumentException("PerHour billing requires DurationHours.");

            if (listing.BillingType == BillingType.PerJob && (scheduledTime != null || durationHours != null))
                throw new ArgumentException("PerJob billing must not have ScheduledTime or DurationHours.");

            if (isSubstitution && substitutingProvider == null)
                throw new ArgumentException("A substitution BookedJob must specify the SubstitutingProvider.");

            if (!isSubstitution && substitutingProvider != null)
                throw new ArgumentException("SubstitutingProvider must be null when IsSubstitution is false.");

            Id = id;
            Listing = listing;
            Neighborhood = neighborhood;
            ScheduledDate = scheduledDate;
            ScheduledTime = scheduledTime;
            DurationHours = durationHours;
            UnitPrice = unitPrice;
            IsSubstitution = isSubstitution;
            SubstitutingProvider = substitutingProvider;
        }

        public ServiceProvider ActualProvider => SubstitutingProvider ?? Listing.Provider;

        public BookedJob CopyWith(
            long? id = null,
            JobListing? listing = null,
            Neighborhood? neighborhood = null,
            DateOnly? scheduledDate = null,
            TimeOnly? scheduledTime = null,
            int? durationHours = null,
            decimal? unitPrice = null,
            bool? isSubstitution = null,
            ServiceProvider? substitutingProvider = null
        )
        {
            return new BookedJob(
                listing ?? Listing,
                neighborhood ?? Neighborhood,
                scheduledDate ?? ScheduledDate,
                unitPrice ?? UnitPrice,
                id ?? Id,
                scheduledTime ?? ScheduledTime,
                durationHours ?? DurationHours,
                isSubstitution ?? IsSubstitution,
                substitutingProvider ?? SubstitutingProvider
            );
        }
    }
}