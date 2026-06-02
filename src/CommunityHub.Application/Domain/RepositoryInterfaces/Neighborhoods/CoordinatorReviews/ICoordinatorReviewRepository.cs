using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews
{
    public interface ICoordinatorReviewRepository
    {
        CoordinatorReview? GetById(long id);
        List<CoordinatorReview> GetByNeighborhoodId(long neighborhoodId);
        public List<CoordinatorReview> GetByCoordinatorId(long coordinatorId);
        int GetRemovedReviewCountByReviewerId(long neighborhoodId, long reviewerId);
        CoordinatorReview Create(CoordinatorReview review);
        void MarkAsRemoved(long reviewId);
    }
}
