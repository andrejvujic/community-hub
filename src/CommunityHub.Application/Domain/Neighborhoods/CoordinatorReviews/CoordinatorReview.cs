using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews
{
    public class CoordinatorReview
    {
        public long Id { get; private set; }
        public int Rating { get; private set; }
        public string? Comment { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Neighborhood Neighborhood { get; private set; }
        public Citizen Reviewer { get; private set; }
        public bool IsRemoved { get; private set; }

        public CoordinatorReview(
            int rating,
            Neighborhood neighborhood,
            Citizen reviewer,
            DateTime createdAt,
            string? comment = null,
            bool isRemoved = false,
            long id = 0
        )
        {
            Id = id;
            Rating = rating;
            Comment = comment;
            CreatedAt = createdAt;
            Neighborhood = neighborhood;
            Reviewer = reviewer;
            IsRemoved = isRemoved;
        }

        public CoordinatorReview CopyWith(
            long? id = null,
            int? rating = null,
            string? comment = null,
            DateTime? createdAt = null,
            Neighborhood? neighborhood = null,
            Citizen? reviewer = null,
            bool? isRemoved = null
        )
        {
            return new CoordinatorReview(
                rating ?? Rating,
                neighborhood ?? Neighborhood,
                reviewer ?? Reviewer,
                createdAt ?? CreatedAt,
                comment ?? Comment,
                isRemoved ?? IsRemoved,
                id ?? Id
            );
        }
    }
}
