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
    }
}
