using CommunityHub.Application.Domain.ServiceBookings.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.ServiceBookings;
using CommunityHub.Application.MappersDto.Services;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Services;

namespace CommunityHub.Application.Services.ServiceBookings
{
    public class BookedJobService
    {
        private readonly IBookedJobRepository _bookedJobRepository
            = Injector.CreateInstance<IBookedJobRepository>();
        private readonly BookedJobMapper _bookedJobMapper = new();
        private readonly JobListingMapper _jobListingMapper = new();
        private readonly AvailableJobSlotMapper _availableJobSlotMapper = new();

        public List<BookedJobDto> GetByNeighborhood(long neighborhoodId)
            => _bookedJobRepository.GetByNeighborhoodId(neighborhoodId)
                .Select(_bookedJobMapper.MapToDto)
                .ToList();

        public AvailableJobSlotDto? FindFirstAvailableSlot(JobListingDto jobListing, int? durationHours)
        {
            var joblisting = _jobListingMapper.MapToDomain(jobListing);
            var occupied = _bookedJobRepository.GetOccupiedSlots(joblisting.Id);
            var availableSlot = joblisting.FindFirstAvailableSlot(occupied, durationHours);
            return availableSlot != null ? _availableJobSlotMapper.MapToDto(availableSlot) : null;
        }
    }
}
