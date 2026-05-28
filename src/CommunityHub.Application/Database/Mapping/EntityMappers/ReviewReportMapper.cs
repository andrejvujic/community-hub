using System.Data;
using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ReviewReportMapper : EntityMapper<ReviewReport>
    {
        private readonly CoordinatorReviewMapper _reviewMapper = new CoordinatorReviewMapper();
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();

        public override ReviewReport? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("review_report_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            CoordinatorReview? review = _reviewMapper.MapToEntity(reader);
            Citizen? reporter = _citizenMapper.MapToEntity(reader);
            if (review is null || reporter is null) return null;

            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("report_created_at"));

            return new ReviewReport(review, reporter, createdAt, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(ReviewReport entity)
        {
            return new Dictionary<string, object?>
            {
                { "review_id", entity.Review.Id },
                { "reporter_id", entity.Reporter.Id },
                { "created_at", entity.CreatedAt }
            };
        }
    }
}
