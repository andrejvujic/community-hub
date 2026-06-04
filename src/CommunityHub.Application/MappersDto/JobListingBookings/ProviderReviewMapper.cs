using CommunityHub.Application.Domain.JobListingBookings;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.ServiceBookings;

namespace CommunityHub.Application.MappersDto.JobListingBookings
{
    public class ProviderReviewMapper : IMapper<ProviderReview, ProviderReviewDto>
    {
        private readonly BookedJobMapper _bookedJobMapper = new();
        private readonly UserMapper _userMapper = new();

        public ProviderReviewDto MapToDto(ProviderReview providerReview)
        {
            if (providerReview == null) throw new ArgumentNullException(nameof(providerReview));

            return new ProviderReviewDto(
                providerReview.Id,
                _bookedJobMapper.MapToDto(providerReview.BookedJob),
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

            return new ProviderReview(
                _bookedJobMapper.MapToEntity(dto.BookedJob),
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
