using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews
{
    public interface ICoordinatorReviewRepository
    {
        CoordinatorReview? GetById(long id);
        List<CoordinatorReview> GetByNeighborhoodId(long neighborhoodId);
        int GetRemovedReviewCountByReviewerId(long neighborhoodId, long reviewerId);
        CoordinatorReview Create(CoordinatorReview review);
        void MarkAsRemoved(long reviewId);
    }
}
