using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.MappersDto.Neighborhoods.CoordinatorReviews
{
    public class CoordinatorReviewMapper : IMapper<CoordinatorReview, CoordinatorReviewDto>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new();
        private readonly CitizenMapper _citizenMapper = new();

        public CoordinatorReviewDto MapToDto(CoordinatorReview coordinatorReview)
        {
            if (coordinatorReview == null) throw new ArgumentNullException(nameof(coordinatorReview));

            return new CoordinatorReviewDto(
                coordinatorReview.Id,
                coordinatorReview.Rating,
                coordinatorReview.Comment,
                coordinatorReview.CreatedAt,
                _neighborhoodMapper.MapToDto(coordinatorReview.Neighborhood),
                _citizenMapper.MapToDto(coordinatorReview.Reviewer),
                coordinatorReview.IsRemoved
            );
        }

        public CoordinatorReview MapToEntity(CoordinatorReviewDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new CoordinatorReview(
                dto.Rating,
                _neighborhoodMapper.MapToEntity(dto.Neighborhood),
                _citizenMapper.MapToEntity(dto.Reviewer),
                dto.CreatedAt,
                dto.Comment,
                dto.IsRemoved,
                dto.Id
            );
        }
    }
}
