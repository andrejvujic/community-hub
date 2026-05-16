using CommunityHub.Application.Domain.ServiceBookings;
using CommunityHub.Application.Domain.ServiceBookings.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.ServiceBookings;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.ServiceBookings;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.ServiceBookings
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

        public ProviderReviewDto Create(BookedJobDto bookedJobDto, UserDto coordinatorDto, 
            int quality, int communication, string? comment)
        {
            var bookedJob = _bookedJobMapper.MapToDomain(bookedJobDto);
            var coordinator = _userMapper.MapToDomain(coordinatorDto);

            var review = new ProviderReview(
                bookedJob, 
                coordinator, 
                quality, 
                communication, 
                DateTime.Now, 
                comment: comment
            );

            var created = _providerReviewRepository.Create(review);
            return _providerReviewMapper.MapToDto(created);
        }

        public ProviderReviewDto Update(ProviderReviewDto providerReviewDto, int quality, int communication, string? comment)
        {
            var review = _providerReviewMapper.MapToDomain(providerReviewDto);
            var updated = review.CopyWith(
                qualityRating: quality,
                communicationRating: communication,
                comment: comment);

            var saved = _providerReviewRepository.Update(updated);
            return _providerReviewMapper.MapToDto(saved);
        }
    }
}
