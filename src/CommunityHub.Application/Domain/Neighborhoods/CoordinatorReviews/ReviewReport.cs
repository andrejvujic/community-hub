using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews
{
    public class ReviewReport
    {
        public long Id { get; private set; }
        public CoordinatorReview Review { get; private set; }
        public Citizen Reporter { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ReviewReport(
            CoordinatorReview review,
            Citizen reporter,
            DateTime createdAt,
            long id = 0
        )
        {
            Id = id;
            Review = review;
            Reporter = reporter;
            CreatedAt = createdAt;
        }

        public ReviewReport CopyWith(
            long? id = null,
            CoordinatorReview? review = null,
            Citizen? reporter = null,
            DateTime? createdAt = null
        )
        {
            return new ReviewReport(
                review ?? Review,
                reporter ?? Reporter,
                createdAt ?? CreatedAt,
                id ?? Id
            );
        }
    }
}
