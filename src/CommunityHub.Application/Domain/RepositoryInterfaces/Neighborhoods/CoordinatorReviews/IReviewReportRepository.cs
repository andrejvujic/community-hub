using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews
{
    public interface IReviewReportRepository
    {
        List<ReviewReport> GetByReviewId(long reviewId);
        int GetReportCountByReviewId(long reviewId);
        bool HasUserReportedReview(long reviewId, long reporterId);
        ReviewReport Create(ReviewReport report);
    }
}
