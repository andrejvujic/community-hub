using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Services
{
    public class JobBookingService
    {
        private readonly JobListingRepository _jobListingRepository;
        private readonly BookedJobRepository _bookedJobRepository;

        public JobBookingService(
            JobListingRepository jobListingRepository,
            BookedJobRepository bookedJobRepository)
        {
            _jobListingRepository = jobListingRepository;
            _bookedJobRepository = bookedJobRepository;
        }

        public List<JobListing> GetJobListingsForLocation(long cityId, long countryId)
            => _jobListingRepository.GetByCityAndCountry(cityId, countryId);

        public List<BookedJob> GetBookedJobListingsForNeighborhood(long neighborhoodId)
            => _bookedJobRepository.GetByNeighborhoodId(neighborhoodId);
    }
}
