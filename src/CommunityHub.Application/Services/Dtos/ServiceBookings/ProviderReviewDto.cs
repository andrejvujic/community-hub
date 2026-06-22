using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.ServiceBookings
{
    public class ProviderReviewDto
    {
        public long Id { get; set; }
        public BookedJobDto BookedJob { get; set; }
        public UserDto Coordinator { get; set; }
        public int QualityRating { get; set; }
        public int CommunicationRating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public float AverageRating => (QualityRating + CommunicationRating) / 2f;
        public bool HasComment => !string.IsNullOrWhiteSpace(Comment);
        public bool IsEditable(DateTime now) => (now - CreatedAt).TotalDays <= 3;

        public static bool CanBeCreatedFor(BookedJobDto bookedJob, DateTime now) =>
            CompletionDateTime(bookedJob) <= now &&
            (now - CompletionDateTime(bookedJob)).TotalDays <= 10;

        private static DateTime CompletionDateTime(BookedJobDto bookedJob)
        {
            if (bookedJob.DurationHours.HasValue && bookedJob.ScheduledTime.HasValue)
                return bookedJob.ScheduledDate
                    .ToDateTime(bookedJob.ScheduledTime.Value)
                    .AddHours(bookedJob.DurationHours.Value);
            return bookedJob.ScheduledDate.ToDateTime(TimeOnly.MaxValue);
        }

        public ProviderReviewDto(
            long id,
            BookedJobDto bookedJob,
            UserDto coordinator,
            int qualityRating,
            int communicationRating,
            DateTime createdAt,
            string? comment = null,
            DateTime? updatedAt = null)
        {
            Id = id;
            BookedJob = bookedJob;
            Coordinator = coordinator;
            QualityRating = qualityRating;
            CommunicationRating = communicationRating;
            Comment = comment;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
