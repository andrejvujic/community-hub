namespace CommunityHub.Application.Services.Dtos.ServiceBookings
{
    /*
    public class BookedJobWithReviewDto
    {
        public BookedJobDto BookedJob { get; }
        public ProviderReviewDto? Review { get; private set; }

        public string ServiceName => BookedJob.Listing.ServiceName;
        public string ProviderName =>
            $"{BookedJob.ActualProvider.Name} {BookedJob.ActualProvider.Surname}";
        public string ScheduledDateTime => BookedJob.ScheduledTime.HasValue
            ? $"{BookedJob.ScheduledDate:dd.MM.yyyy.} u {BookedJob.ScheduledTime.Value:HH:mm} h"
            : $"{BookedJob.ScheduledDate:dd.MM.yyyy.}";
        public string TotalPrice => BookedJob.DurationHours.HasValue
            ? $"{BookedJob.TotalPrice:N2} RSD ({BookedJob.DurationHours}h)"
            : $"{BookedJob.TotalPrice:N2} RSD";
        public string Status => BookedJob.IsFinished(DateTime.Now) ? "Izvršeno" : "Zakazano";
        public string Rating => Review is not null
            ? $"{Review.AverageRating:F2}"
            : "/";

        public bool CanReview =>
            Review is null &&
            BookedJob.IsFinished(DateTime.Now) &&
            ProviderReviewDto.CanBeCreatedFor(BookedJob, DateTime.Now);

        public bool CanEditReview =>
            Review is not null &&
            Review.IsEditable(DateTime.Now);

        public BookedJobWithReviewDto(BookedJobDto bookedJob, ProviderReviewDto? review)
        {
            BookedJob = bookedJob;
            Review = review;
        }

        public void SetReview(ProviderReviewDto review) => Review = review;
    }
    */
}
