using CommunityHub.Application.Domain.JobListingBookings;
using CommunityHub.Application.MappersDto.JobListings;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.ServiceBookings;

namespace CommunityHub.Application.MappersDto.JobListingBookings
{
    public class BookedJobMapper : IMapper<BookedJob, BookedJobDto>
    {
        private readonly JobListingMapper _listingMapper = new();
        private readonly NeighborhoodMapper _neighborhoodMapper = new();
        private readonly ServiceProviderMapper _providerMapper = new();

        public BookedJobDto MapToDto(BookedJob bookedJob)
        {
            if (bookedJob == null) throw new ArgumentNullException(nameof(bookedJob));

            return new BookedJobDto(
                bookedJob.Id,
                _listingMapper.MapToDto(bookedJob.Listing),
                _neighborhoodMapper.MapToDto(bookedJob.Neighborhood),
                bookedJob.ScheduledDate,
                bookedJob.UnitPrice,
                bookedJob.ScheduledTime,
                bookedJob.DurationHours,
                bookedJob.IsSubstitution,
                bookedJob.SubstitutingProvider != null
                    ? _providerMapper.MapToDto(bookedJob.SubstitutingProvider)
                    : null);
        }

        public BookedJob MapToEntity(BookedJobDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new BookedJob(
                _listingMapper.MapToEntity(dto.Listing),
                _neighborhoodMapper.MapToEntity(dto.Neighborhood),
                dto.ScheduledDate,
                dto.UnitPrice,
                dto.Id,
                dto.ScheduledTime,
                dto.DurationHours,
                dto.IsSubstitution,
                dto.SubstitutingProvider != null
                    ? _providerMapper.MapToEntity(dto.SubstitutingProvider)
                    : null);
        }
    }
}