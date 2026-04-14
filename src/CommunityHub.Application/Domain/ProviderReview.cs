namespace CommunityHub.Application.Domain
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

        public bool IsEditable => (DateTime.Now - CreatedAt).TotalDays <= 3;

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
                    $"Only a NeighborhoodCoordinator may review a service provider, but got role '{coordinator.Role}'.",
                    nameof(coordinator));

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