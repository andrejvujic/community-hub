using CommunityHub.Application.Domain.Services;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.ServiceBookings
{
    public class ProviderReview
    {
        public long Id { get; private set; }
        public BookedJob BookedJob { get; private set; }
        public User Coordinator { get; private set; }
        public int QualityRating { get; private set; }
        public int CommunicationRating { get; private set; }
        public string? Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public decimal AverageRating => (QualityRating + CommunicationRating) / 2m;

        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);

        public bool IsEditable(DateTime now) => (now - CreatedAt).TotalDays <= 3;

        public static bool CanBeCreatedFor(BookedJob bookedJob, DateTime now) =>
            CompletionDateTime(bookedJob) <= now &&
            (now - CompletionDateTime(bookedJob)).TotalDays <= 10;

        private static DateTime CompletionDateTime(BookedJob bookedJob)
        {
            if (bookedJob.Listing.BillingType == BillingType.PerHour
                && bookedJob.ScheduledTime.HasValue
                && bookedJob.DurationHours.HasValue)
            {
                return bookedJob.ScheduledDate
                    .ToDateTime(bookedJob.ScheduledTime.Value)
                    .AddHours(bookedJob.DurationHours.Value);
            }

            return bookedJob.ScheduledDate.ToDateTime(TimeOnly.MaxValue);
        }

        public ProviderReview(
            BookedJob bookedJob,
            User coordinator,
            int qualityRating,
            int communicationRating,
            DateTime createdAt,
            long id = 0,
            string? comment = null,
            DateTime? updatedAt = null
        )
        {
            if (coordinator.Role != Role.NeighborhoodCoordinator)
                throw new ArgumentException(
                    $"Recenziju može ostaviti samo koordinator kvarta, ali je prosleđena uloga '{coordinator.Role}'.",
                    nameof(coordinator));

            if (qualityRating < 1 || qualityRating > 5)
                throw new ArgumentOutOfRangeException(
                    nameof(qualityRating),
                    qualityRating,
                    "Ocena kvaliteta mora biti između 1 i 5.");

            if (communicationRating < 1 || communicationRating > 5)
                throw new ArgumentOutOfRangeException(
                    nameof(communicationRating),
                    communicationRating,
                    "Ocena komunikacije mora biti između 1 i 5.");

            Id = id;
            BookedJob = bookedJob;
            Coordinator = coordinator;
            QualityRating = qualityRating;
            CommunicationRating = communicationRating;
            Comment = comment;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public ProviderReview CopyWith(
            long? id = null,
            BookedJob? bookedJob = null,
            User? coordinator = null,
            int? qualityRating = null,
            int? communicationRating = null,
            string? comment = null,
            DateTime? createdAt = null,
            DateTime? updatedAt = null
        )
        {
            return new ProviderReview(
                bookedJob ?? BookedJob,
                coordinator ?? Coordinator,
                qualityRating ?? QualityRating,
                communicationRating ?? CommunicationRating,
                createdAt ?? CreatedAt,
                id ?? Id,
                comment ?? Comment,
                updatedAt ?? UpdatedAt
            );
        }
    }
}