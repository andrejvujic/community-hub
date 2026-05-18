namespace CommunityHub.Application.Domain.ServiceBookings.RepositoryInterfaces
{
    public interface IBookedJobRepository
    {
        public List<BookedJob> GetByNeighborhoodId(long neighborhoodId);
        public BookedJob Create(BookedJob bookedJob);
        public List<(DateOnly Date, TimeOnly? Time, int? DurationHours)> GetOccupiedSlots(long jobListingId);
    }
}
