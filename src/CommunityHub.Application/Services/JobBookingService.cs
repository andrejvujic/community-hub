using CommunityHub.Application.Database.Repositories;

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
    }
}
