using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.MappersDto.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Services.Neighborhoods.CoordinatorReviews
{
    public class ReviewReportService
    {
        private readonly IReviewReportRepository _reviewReportRepository = Injector.CreateInstance<IReviewReportRepository>();
        private readonly ReviewReportMapper _reportMapper = new();

        public int GetReportCountByReviewId(long reviewId)
            => _reviewReportRepository.GetReportCountByReviewId(reviewId);

        public List<ReviewReportDto> GetByReviewId(long reviewId)
        {
            var reports = _reviewReportRepository.GetByReviewId(reviewId);
            return reports.Select(_reportMapper.MapToDto).ToList();
        }

        public bool HasUserReportedReview(long reviewId, long reporterId)
            => _reviewReportRepository.HasUserReportedReview(reviewId, reporterId);

        public ReviewReportDto Create(ReviewReportDto reportDto)
        {
            var report = _reportMapper.MapToEntity(reportDto);
            var created = _reviewReportRepository.Create(report);
            return _reportMapper.MapToDto(created);
        }
    }
}
