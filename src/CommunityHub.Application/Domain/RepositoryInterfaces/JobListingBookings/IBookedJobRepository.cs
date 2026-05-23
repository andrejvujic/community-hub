using CommunityHub.Application.Domain.JobListingBookings;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.JobListingBookings
{
    public interface IBookedJobRepository
    {
        public List<BookedJob> GetByNeighborhoodId(long neighborhoodId);
        public BookedJob Create(BookedJob bookedJob);
        public List<BookedJob> GetByServiceProviderId(long serviceProviderId);
        public List<(DateOnly Date, TimeOnly? Time, int? DurationHours)> GetOccupiedSlots(long jobListingId);
    }
}
