using CommunityHub.Application.Domain.JobListingBookings;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.JobListingBookings
{
    public interface IProviderReviewRepository
    {
        public List<ProviderReview> GetByProviderId(long providerId);
        public ProviderReview? GetByBookedJobId(long bookedJobId);
        public ProviderReview Create(ProviderReview review);
        public ProviderReview Update(ProviderReview review);
        public decimal? GetAverageRatingForProvider(long providerId);
        public Dictionary<long, ProviderReview> GetAllByNeighborhood(long neighborhoodId);
    }
}
