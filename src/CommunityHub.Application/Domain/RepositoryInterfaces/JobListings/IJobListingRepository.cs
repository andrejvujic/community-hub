using CommunityHub.Application.Domain.JobListings;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.JobListings
{
    public interface IJobListingRepository
    {
        public List<JobListing> GetByProviderId(long providerId);
        public JobListing Create(JobListing listing);
        public PriceEntry AddPriceEntry(PriceEntry entry);
        public List<JobListing> GetByCityAndCountry(long cityId, long countryId);
    }
}
