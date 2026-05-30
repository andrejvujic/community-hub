using CommunityHub.Application.Database.Repositories.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.MappersDto.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Services.Neighborhoods
{
    public class ReviewReportService
    {
        private readonly IReviewReportRepository _reviewReportRepository = new ReviewReportRepository();
        private readonly ReviewReportMapper _reportMapper = new();

        public int GetReportCountByReviewId(long reviewId)
            => _reviewReportRepository.GetReportCountByReviewId(reviewId);

        public ReviewReportDto? GetById(long reportId)
        {
            var report = _reviewReportRepository.GetById(reportId);
            return report != null ? _reportMapper.MapToDto(report) : null;
        }

        public ReviewReportDto Create(ReviewReportDto reportDto)
        {
            var report = _reportMapper.MapToEntity(reportDto);
            var created = _reviewReportRepository.Create(report);
            return _reportMapper.MapToDto(created);
        }
    }
}
