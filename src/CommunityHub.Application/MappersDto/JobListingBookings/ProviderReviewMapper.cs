using CommunityHub.Application.Domain.JobListingBookings;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.ServiceBookings;

namespace CommunityHub.Application.MappersDto.JobListingBookings
{
    public class ProviderReviewMapper : IMapper<ProviderReview, ProviderReviewDto>
    {
        private readonly UserMapper _userMapper = new();
        private readonly bool _mapReview;

        public ProviderReviewMapper(bool mapReview = true)
        {
            _mapReview = mapReview;
        }

        public ProviderReviewDto MapToDto(ProviderReview providerReview)
        {
            if (providerReview == null) throw new ArgumentNullException(nameof(providerReview));

            var bookedJobDto = new BookedJobMapper(mapReview: false).MapToDto(providerReview.BookedJob);

            return new ProviderReviewDto(
                providerReview.Id,
                bookedJobDto,
                _userMapper.MapToDto(providerReview.Coordinator),
                providerReview.QualityRating,
                providerReview.CommunicationRating,
                providerReview.CreatedAt,
                providerReview.Comment,
                providerReview.UpdatedAt);
        }

        public ProviderReview MapToEntity(ProviderReviewDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var bookedJob = new BookedJobMapper(mapReview: false).MapToEntity(dto.BookedJob);

            return new ProviderReview(
                bookedJob,
                _userMapper.MapToEntity(dto.Coordinator),
                dto.QualityRating,
                dto.CommunicationRating,
                dto.CreatedAt,
                dto.Id,
                dto.Comment,
                dto.UpdatedAt);
        }
    }
}
