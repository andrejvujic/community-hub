using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews
{
    public class CoordinatorReviewDto
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public NeighborhoodDto Neighborhood { get; set; }
        public CitizenDto Reviewer { get; set; }
        public bool IsRemoved { get; set; }

        public CoordinatorReviewDto(
            long id,
            int rating,
            string? comment,
            DateTime createdAt,
            NeighborhoodDto neighborhood,
            CitizenDto reviewer,
            bool isRemoved
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
    }
}
