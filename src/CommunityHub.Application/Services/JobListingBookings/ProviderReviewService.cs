using CommunityHub.Application.Domain.JobListingBookings;
using CommunityHub.Application.MappersDto.JobListingBookings;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Users;
using CommunityHub.Application.Domain.RepositoryInterfaces.JobListingBookings;

namespace CommunityHub.Application.Services.JobListingBookings
{
    public class ProviderReviewService
    {
        private readonly IProviderReviewRepository _providerReviewRepository
            = Injector.CreateInstance<IProviderReviewRepository>();
        private readonly ProviderReviewMapper _providerReviewMapper = new();
        private readonly UserMapper _userMapper = new();
        private readonly BookedJobMapper _bookedJobMapper = new();

        public ProviderReviewDto? GetByBookedJobId(long bookedJobId)
        {
            var review = _providerReviewRepository.GetByBookedJobId(bookedJobId);
            return review is null ? null : _providerReviewMapper.MapToDto(review);
        }

        public Dictionary<long, ProviderReviewDto> GetAllByNeighborhood(long neighborhoodId)
            => _providerReviewRepository.GetAllByNeighborhood(neighborhoodId)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => _providerReviewMapper.MapToDto(kvp.Value));

        public ProviderReviewDto Create(BookedJobDto bookedJobDto, UserDto coordinatorDto,
            int quality, int communication, string comment)
        {
            var bookedJob = _bookedJobMapper.MapToEntity(bookedJobDto);
            var coordinator = _userMapper.MapToEntity(coordinatorDto);

            var review = bookedJob.CreateReview(coordinator, quality, communication, comment, DateTime.Now);

            var created = _providerReviewRepository.Create(review);
            return _providerReviewMapper.MapToDto(created);
        }

        public ProviderReviewDto Update(ProviderReviewDto providerReviewDto,
            int quality, int communication, string comment)
        {
            var review = _providerReviewMapper.MapToEntity(providerReviewDto);

            review.Update(quality, communication, comment, DateTime.Now);

            var saved = _providerReviewRepository.Update(review);
            return _providerReviewMapper.MapToDto(saved);
        }
    }
}
