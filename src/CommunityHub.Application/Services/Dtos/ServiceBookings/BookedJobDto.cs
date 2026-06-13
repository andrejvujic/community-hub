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
        public ProviderReviewDto? Review { get; set; }

        public string ProviderName =>
            $"{ActualProvider.Name} {ActualProvider.Surname}";

        public string ScheduledDateTime => ScheduledTime.HasValue
            ? $"{ScheduledDate:dd.MM.yyyy.} u {ScheduledTime.Value:HH:mm} h"
            : $"{ScheduledDate:dd.MM.yyyy.}";

        public string RatingView => Review is not null
            ? $"{Review.AverageRating:F2}"
            : "/";

        public string Status { get; set; }
        
        public decimal TotalPrice => DurationHours.HasValue
            ? UnitPrice * DurationHours.Value
            : UnitPrice;

        public string TotalPriceView => DurationHours.HasValue
            ? $"{TotalPrice:N2} RSD ({DurationHours}h)"
            : $"{TotalPrice:N2} RSD";

        public BookedJobDto(
            long id,
            JobListingDto listing,
            NeighborhoodDto neighborhood,
            DateOnly scheduledDate,
            decimal unitPrice,
            string status,
            TimeOnly? scheduledTime = null,
            int? durationHours = null,
            bool isSubstitution = false,
            ServiceProviderDto? substitutingProvider = null,
            ProviderReviewDto? review = null)
        {
            Id = id;
            Listing = listing;
            Neighborhood = neighborhood;
            ScheduledDate = scheduledDate;
            ScheduledTime = scheduledTime;
            DurationHours = durationHours;
            UnitPrice = unitPrice;
            Status = status;
            IsSubstitution = isSubstitution;
            SubstitutingProvider = substitutingProvider;
            Review = review;
        }
    }
}
