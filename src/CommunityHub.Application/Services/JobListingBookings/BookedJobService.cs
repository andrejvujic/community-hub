using CommunityHub.Application.Domain.JobListingBookings;
using CommunityHub.Application.MappersDto.JobListingBookings;
using CommunityHub.Application.MappersDto.JobListings;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Services;
using CommunityHub.Application.Domain.RepositoryInterfaces.JobListingBookings;

namespace CommunityHub.Application.Services.JobListingBookings
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

        public bool CanReview(BookedJobDto dto)
        {
            var entity = _bookedJobMapper.MapToEntity(dto);
            return entity != null && entity.CanReviewBeCreated(DateTime.Now);
        }

        public bool CanEditReview(BookedJobDto dto)
        {
            var entity = _bookedJobMapper.MapToEntity(dto);
            return entity != null && entity.CanReviewBeEdited(DateTime.Now);
        }

        public AvailableJobSlotDto? FindFirstAvailableSlot(JobListingDto jobListing, int? durationHours)
        {
            var joblisting = _jobListingMapper.MapToEntity(jobListing);
            var bookedJobs = _bookedJobRepository.GetByServiceProviderId(joblisting.Provider.Id);
            var availableSlot = joblisting.FindFirstAvailableSlot(bookedJobs, durationHours);
            return availableSlot != null ? _availableJobSlotMapper.MapToDto(availableSlot) : null;
        }

        public BookedJobDto Book(JobListingDto listingDto, NeighborhoodDto neighborhoodDto, AvailableJobSlotDto slotDto)
        {
            var listing = _jobListingMapper.MapToEntity(listingDto);
            var neighborhood = _neighborhoodMapper.MapToEntity(neighborhoodDto);
            var slot = _availableJobSlotMapper.MapToEntity(slotDto);
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

        private List<BookedJob> GetAllFinishedByCoordinator(long coordinatorId)
            => _bookedJobRepository.GetAllFinishedByCoordinatorId(coordinatorId);

        public List<BookedJobDto> GetRecentUnreviewedByCoordinator(long coordinatorId)
            => GetAllFinishedByCoordinator(coordinatorId)
                .Where(j => j.CanReviewBeCreated(DateTime.Now))
                .Select(_bookedJobMapper.MapToDto)
                .ToList();

        public List<BookedJobDto> GetEditableByCoordinator(long coordinatorId)
            => GetAllFinishedByCoordinator(coordinatorId)
                .Where(j => j.CanReviewBeEdited(DateTime.Now))
                .Select(_bookedJobMapper.MapToDto)
                .ToList();
    }
}
