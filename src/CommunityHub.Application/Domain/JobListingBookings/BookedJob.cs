using CommunityHub.Application.Domain.JobListings;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.JobListingBookings
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
        public bool IsSubstitution { get; private set; }
        public ServiceProvider? SubstitutingProvider { get; private set; }
        public ProviderReview? Review { get; private set; }

        public decimal TotalPrice => DurationHours.HasValue
            ? UnitPrice * DurationHours.Value
            : UnitPrice;

        public ServiceProvider ActualProvider => SubstitutingProvider ?? Listing.Provider;

        public bool CanReviewBeCreated(DateTime now)
        {
            if (Review != null) return false;
            return CompletionDateTime() <= now && (now - CompletionDateTime()).TotalDays <= 10;
        }

        public bool CanReviewBeEdited(DateTime now)
        {
            if (Review == null) return false;
            return (now - Review.CreatedAt).TotalDays <= 3;
        }

        private DateTime CompletionDateTime()
        {
            if (Listing.BillingType == BillingType.PerHour
                && ScheduledTime.HasValue
                && DurationHours.HasValue)
            {
                return ScheduledDate.ToDateTime(ScheduledTime.Value).AddHours(DurationHours.Value);
            }
            return ScheduledDate.ToDateTime(new TimeOnly(23, 59, 59));
        }

        public bool IsFinished(DateTime at)
        {
            if (DurationHours.HasValue && ScheduledTime.HasValue)
            {
                return ScheduledDate.ToDateTime(ScheduledTime.Value).AddHours(DurationHours.Value) <= at;
            }
            return ScheduledDate < DateOnly.FromDateTime(at);
        }

        public ProviderReview CreateReview(
            User coordinator,
            int qualityRating,
            int communicationRating,
            string comment,
            DateTime now)
        {
            if (!CanReviewBeCreated(now))
                throw new InvalidOperationException(
                    "Ocenjivanje nije moguće. Ova usluga ne može biti ocenjena.");

            var review = new ProviderReview(this, coordinator, qualityRating, communicationRating, now, comment: comment);
            Review = review;
            return review;
        }

        public BookedJob(
            JobListing listing,
            Neighborhood neighborhood,
            DateOnly scheduledDate,
            decimal unitPrice,
            long id = 0,
            TimeOnly? scheduledTime = null,
            int? durationHours = null,
            bool isSubstitution = false,
            ServiceProvider? substitutingProvider = null,
            ProviderReview? review = null
        )
        {
            ValidateBillingConsistency(listing, scheduledTime, durationHours);
            ValidateSubstitution(isSubstitution, substitutingProvider);

            Id = id;
            Listing = listing;
            Neighborhood = neighborhood;
            ScheduledDate = scheduledDate;
            ScheduledTime = scheduledTime;
            DurationHours = durationHours;
            UnitPrice = unitPrice;
            IsSubstitution = isSubstitution;
            SubstitutingProvider = substitutingProvider;
            Review = review;
        }

        public BookedJob CopyWith(
            long? id = null,
            JobListing? listing = null,
            Neighborhood? neighborhood = null,
            DateOnly? scheduledDate = null,
            TimeOnly? scheduledTime = null,
            int? durationHours = null,
            decimal? unitPrice = null,
            bool? isSubstitution = null,
            ServiceProvider? substitutingProvider = null,
            ProviderReview? review = null
        )
        {
            var newIsSubstitution = isSubstitution ?? IsSubstitution;
            var newSubstitutingProvider = DetermineSubstitutingProvider(newIsSubstitution, substitutingProvider);

            return new BookedJob(
                listing ?? Listing,
                neighborhood ?? Neighborhood,
                scheduledDate ?? ScheduledDate,
                unitPrice ?? UnitPrice,
                id ?? Id,
                scheduledTime ?? ScheduledTime,
                durationHours ?? DurationHours,
                newIsSubstitution,
                newSubstitutingProvider,
                review ?? Review
            );
        }

        private ServiceProvider? DetermineSubstitutingProvider(bool isSubstitution, ServiceProvider? incomingProvider) =>
            incomingProvider ?? (isSubstitution ? SubstitutingProvider : null);

        private static void ValidateBillingConsistency(
            JobListing listing,
            TimeOnly? scheduledTime,
            int? durationHours)
        {
            if (listing.BillingType == BillingType.PerHour && (durationHours == null || scheduledTime == null))
                throw new ArgumentException("Usluga koja se naplaćuje po satu mora imati naveden broj sati trajanja i vreme početka.");
        }

        private static void ValidateSubstitution(bool isSubstitution, ServiceProvider? substitutingProvider)
        {
            if (isSubstitution && substitutingProvider == null)
                throw new ArgumentException("Zakazana usluga označena kao zamena mora imati navedenog servisera zamene.");

            if (!isSubstitution && substitutingProvider != null)
                throw new ArgumentException("Serviser zamene mora biti null kada zakazana usluga nije zamena.");
        }
    }
}