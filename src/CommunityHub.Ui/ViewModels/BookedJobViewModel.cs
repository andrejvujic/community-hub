using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Ui.ViewModels
{
    public class BookedJobViewModel
    {
        public BookedJob BookedJob { get; }
        public ProviderReview? Review { get; }

        public string ServiceName => BookedJob.Listing.ServiceName;
        public string ProviderName =>
            $"{BookedJob.ActualProvider.Name} {BookedJob.ActualProvider.Surname}";
        public string ScheduledDateTime => BookedJob.ScheduledTime.HasValue
            ? $"{BookedJob.ScheduledDate:dd.MM.yyyy.} u {BookedJob.ScheduledTime.Value:HH:mm}"
            : $"{BookedJob.ScheduledDate:dd.MM.yyyy.}";
        public string TotalPrice => BookedJob.DurationHours.HasValue
            ? $"{BookedJob.TotalPrice:N2} RSD ({BookedJob.DurationHours}h)"
            : $"{BookedJob.TotalPrice:N2} RSD";
        public string Status => BookedJob.ScheduledDate < DateOnly.FromDateTime(DateTime.Today)
            ? "Izvršeno"
            : "Zakazano";
        public string Rating => Review is not null
            ? $"{Review.AverageRating:F2}"
            : "/";
        public bool CanReview => ProviderReview.CanBeCreatedFor(BookedJob, DateTime.Now)
            && Review is null;
        public bool CanEditReview => Review is not null && Review.IsEditable(DateTime.Now);

        public BookedJobViewModel(BookedJob bookedJob, ProviderReview? review = null)
        {
            BookedJob = bookedJob;
            Review = review;
        }
    }
}
