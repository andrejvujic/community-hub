using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews
{
    public interface IReviewReportRepository
    {
        ReviewReport? GetById(long id);
        int GetReportCountByReviewId(long reviewId);
        ReviewReport Create(ReviewReport report);
    }
}
