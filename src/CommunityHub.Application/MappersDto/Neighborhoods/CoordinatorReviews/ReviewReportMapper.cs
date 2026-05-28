using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.MappersDto.Neighborhoods.CoordinatorReviews
{
    public class ReviewReportMapper : IMapper<ReviewReport, ReviewReportDto>
    {
        private readonly CoordinatorReviewMapper _reviewMapper = new();
        private readonly CitizenMapper _citizenMapper = new();

        public ReviewReportDto MapToDto(ReviewReport reviewReport)
        {
            if (reviewReport == null) throw new ArgumentNullException(nameof(reviewReport));

            return new ReviewReportDto(
                reviewReport.Id,
                _reviewMapper.MapToDto(reviewReport.Review),
                _citizenMapper.MapToDto(reviewReport.Reporter),
                reviewReport.CreatedAt
            );
        }

        public ReviewReport MapToEntity(ReviewReportDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new ReviewReport(
                _reviewMapper.MapToEntity(dto.Review),
                _citizenMapper.MapToEntity(dto.Reporter),
                dto.CreatedAt,
                dto.Id
            );
        }
    }
}
