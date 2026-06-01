using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.MappersDto.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Services.Neighborhoods.CoordinatorReviews
{
    public class CoordinatorReviewService
    {
        private readonly ICoordinatorReviewRepository _coordinatorReviewRepository = Injector.CreateInstance<ICoordinatorReviewRepository>();
        private readonly IReviewReportRepository _reportRepository = Injector.CreateInstance<IReviewReportRepository>();
        private readonly CoordinatorReviewMapper _reviewMapper = new();

        public List<CoordinatorReviewDto> GetByNeighborhoodId(long neighborhoodId)
        {
            var reviews = _coordinatorReviewRepository.GetByNeighborhoodId(neighborhoodId).Select(_reviewMapper.MapToDto).ToList();

            foreach (var review in reviews)
            {
                review.ReportCount = _reportRepository.GetReportCountByReviewId(review.Id);
            }

            return reviews;
        }

        public List<CoordinatorReviewDto> GetByCoordinatorId(long coordinatorId)
            => _coordinatorReviewRepository.GetByCoordinatorId(coordinatorId)
                .Select(_reviewMapper.MapToDto)
                .ToList();

        public int GetRemovedReviewCountByReviewerId(long neighborhoodId, long reviewerId)
        {
            return _coordinatorReviewRepository.GetRemovedReviewCountByReviewerId(neighborhoodId, reviewerId);
        }

        public CoordinatorReviewDto? GetById(long reviewId)
        {
            var review = _coordinatorReviewRepository.GetById(reviewId);
            if (review == null) return null;

            var dto = _reviewMapper.MapToDto(review);
            dto.ReportCount = _reportRepository.GetReportCountByReviewId(reviewId);
            return dto;
        }

        public CoordinatorReviewDto Create(CoordinatorReviewDto reviewDto)
        {
            var review = _reviewMapper.MapToEntity(reviewDto);
            var created = _coordinatorReviewRepository.Create(review);
            var dto = _reviewMapper.MapToDto(created);
            dto.ReportCount = 0;
            return dto;
        }

        public void MarkAsRemoved(long reviewId) => _coordinatorReviewRepository.MarkAsRemoved(reviewId);
    }
}

