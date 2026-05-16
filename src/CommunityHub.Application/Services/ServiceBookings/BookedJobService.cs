using CommunityHub.Application.Domain.ServiceBookings;
using CommunityHub.Application.Domain.ServiceBookings.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.MappersDto.ServiceBookings;
using CommunityHub.Application.MappersDto.Services;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
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
        private readonly NeighborhoodMapper _neighborhoodMapper = new();

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

        public BookedJobDto Book(JobListingDto listingDto, NeighborhoodDto neighborhoodDto, AvailableJobSlotDto slotDto)
        {
            var listing = _jobListingMapper.MapToDomain(listingDto);
            var neighborhood = _neighborhoodMapper.MapToDomain(neighborhoodDto);
            var slot = _availableJobSlotMapper.MapToDomain(slotDto);
            var job = new BookedJob(
                listing, 
                neighborhood, 
                slot.Date, 
                slot.Price,
                scheduledTime: slot.Time, 
                durationHours: slot.DurationHours
            );
            var created = _bookedJobRepository.Create(job);
            return _bookedJobMapper.MapToDto(created);
        }
    }
}
