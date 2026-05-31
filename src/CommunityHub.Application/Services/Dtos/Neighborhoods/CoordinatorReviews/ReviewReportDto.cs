using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews
{
    public class ReviewReportDto
    {
        public long Id { get; set; }
        public CoordinatorReviewDto Review { get; set; }
        public CitizenDto Reporter { get; set; }
        public DateTime CreatedAt { get; set; }

        public ReviewReportDto(
            long id,
            CoordinatorReviewDto review,
            CitizenDto reporter,
            DateTime createdAt
        )
        {
            Id = id;
            Review = review;
            Reporter = reporter;
            CreatedAt = createdAt;
        }
    }
}
