namespace CommunityHub.Application.Domain.JobListings.RepositoryInterfaces
{
    public interface IJobListingRepository
    {
        public List<JobListing> GetByProviderId(long providerId);
        public JobListing Create(JobListing listing);
        public PriceEntry AddPriceEntry(PriceEntry entry);
        public List<JobListing> GetByCityAndCountry(long cityId, long countryId);
    }
}
