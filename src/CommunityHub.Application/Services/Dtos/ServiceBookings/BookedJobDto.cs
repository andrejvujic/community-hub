using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Services;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.ServiceBookings
{
    public class BookedJobDto
    {
        public long Id { get; set; }
        public JobListingDto Listing { get; set; }
        public NeighborhoodDto Neighborhood { get; set; }
        public DateOnly ScheduledDate { get; set; }
        public TimeOnly? ScheduledTime { get; set; }
        public int? DurationHours { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsSubstitution { get; set; }
        public ServiceProviderDto? SubstitutingProvider { get; set; }
        public ServiceProviderDto ActualProvider => SubstitutingProvider ?? Listing.Provider;
        public decimal TotalPrice => DurationHours.HasValue
            ? UnitPrice * DurationHours.Value
            : UnitPrice;

        public bool IsFinished(DateTime at)
        {
            if (DurationHours.HasValue && ScheduledTime.HasValue)
                return ScheduledDate.ToDateTime(ScheduledTime.Value).AddHours(DurationHours.Value) <= at;
            return ScheduledDate < DateOnly.FromDateTime(at);
        }

        public BookedJobDto(
            long id,
            JobListingDto listing,
            NeighborhoodDto neighborhood,
            DateOnly scheduledDate,
            decimal unitPrice,
            TimeOnly? scheduledTime = null,
            int? durationHours = null,
            bool isSubstitution = false,
            ServiceProviderDto? substitutingProvider = null)
        {
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
    }
}
