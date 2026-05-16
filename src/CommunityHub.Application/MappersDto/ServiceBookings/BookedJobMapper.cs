using CommunityHub.Application.Domain.ServiceBookings;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.MappersDto.Services;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.ServiceBookings;

namespace CommunityHub.Application.MappersDto.ServiceBookings
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

        public BookedJob MapToDomain(BookedJobDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new BookedJob(
                _listingMapper.MapToDomain(dto.Listing),
                _neighborhoodMapper.MapToDomain(dto.Neighborhood),
                dto.ScheduledDate,
                dto.UnitPrice,
                dto.Id,
                dto.ScheduledTime,
                dto.DurationHours,
                dto.IsSubstitution,
                dto.SubstitutingProvider != null
                    ? _providerMapper.MapToDomain(dto.SubstitutingProvider)
                    : null);
        }
    }
}