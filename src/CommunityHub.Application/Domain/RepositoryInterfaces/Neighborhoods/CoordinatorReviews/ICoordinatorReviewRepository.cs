using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews
{
    public interface ICoordinatorReviewRepository
    {
        CoordinatorReview? GetById(long id);
        List<CoordinatorReview> GetByNeighborhoodId(long neighborhoodId);
        CoordinatorReview Create(CoordinatorReview review);
        void MarkAsRemoved(long reviewId);
    }
}
