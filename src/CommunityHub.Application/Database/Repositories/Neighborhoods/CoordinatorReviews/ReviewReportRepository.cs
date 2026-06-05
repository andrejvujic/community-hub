using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Database.Repositories.Neighborhoods.CoordinatorReviews
{
    public class ReviewReportRepository : IReviewReportRepository
    {
        private readonly ReviewReportMapper _reportMapper = new ReviewReportMapper();

        private const string SelectColumns = @"
            review_report.id AS review_report_id,
            review_report.created_at AS report_created_at,

            coordinator_review.id AS coordinator_review_id,
            coordinator_review.rating,
            coordinator_review.comment,
            coordinator_review.created_at,
            coordinator_review.is_removed,

            neighborhood.id AS neighborhood_id,
            neighborhood.name AS neighborhood_name,
            neighborhood.description,
            neighborhood.budget,

            neighborhood_city.id AS city_id,
            neighborhood_city.name AS city_name,

            neighborhood_country.id AS country_id,
            neighborhood_country.name AS country_name,

            coordinator.id AS coordinator_user_id,
            coordinator.username AS coordinator_username,
            coordinator.password AS coordinator_password,
            coordinator.name AS coordinator_name,
            coordinator.surname AS coordinator_surname,
            coordinator.user_role AS coordinator_user_role,

            reviewer.id AS user_id,
            reviewer.username,
            reviewer.password,
            reviewer.name,
            reviewer.surname,
            reviewer.user_role,

            reviewer_address.id AS address_id,
            reviewer_address.street_number,
            reviewer_address.neighborhood,

            reviewer_street.id AS street_id,
            reviewer_street.name AS street_name,

            reviewer_city.id AS city_id,
            reviewer_city.name AS city_name,

            reviewer_country.id AS country_id,
            reviewer_country.name AS country_name,

            reviewer_membership.id AS membership_id,
            reviewer_membership.acceptance_date,

            reporter.id AS reporter_id,
            reporter.username AS reporter_username,
            reporter.password AS reporter_password,
            reporter.name AS reporter_name,
            reporter.surname AS reporter_surname,
            reporter.user_role AS reporter_user_role";

        private const string FromJoins = @"
            FROM review_reports review_report
            INNER JOIN coordinator_reviews coordinator_review ON coordinator_review.id = review_report.review_id
            INNER JOIN neighborhoods neighborhood ON neighborhood.id = coordinator_review.neighborhood_id
            INNER JOIN cities neighborhood_city ON neighborhood_city.id = neighborhood.city_id
            INNER JOIN countries neighborhood_country ON neighborhood_country.id = neighborhood_city.country_id
            INNER JOIN users coordinator ON coordinator.id = neighborhood.coordinator_id
            INNER JOIN citizens reviewer ON reviewer.id = coordinator_review.reviewer_id
            INNER JOIN addresses reviewer_address ON reviewer.address_id = reviewer_address.id
            INNER JOIN streets reviewer_street ON reviewer_address.street_id = reviewer_street.id
            INNER JOIN cities reviewer_city ON reviewer_address.city_id = reviewer_city.id
            INNER JOIN countries reviewer_country ON reviewer_city.country_id = reviewer_country.id
            LEFT JOIN neighborhood_memberships reviewer_membership ON reviewer.membership_id = reviewer_membership.id
            INNER JOIN citizens reporter ON reporter.id = review_report.reporter_id";

        public List<ReviewReport> GetByReviewId(long reviewId)
        {
            using var connection = DatabaseConnection.Create();
            var query = DatabaseQuery.Create(
                $"SELECT {SelectColumns} {FromJoins} WHERE coordinator_review.id = @review_id ORDER BY review_report.created_at DESC"
            ).AddParameter("review_id", reviewId);

            var reports = new List<ReviewReport>();
            connection.ExecuteReader(query, reader =>
            {
                reports.Add(_reportMapper.MapToEntity(reader));
            });
            return reports;
        }

        public int GetReportCountByReviewId(long reviewId)
        {
            using var connection = DatabaseConnection.Create();
            var query = DatabaseQuery.Create(
                "SELECT COUNT(*) FROM review_reports WHERE review_id = @review_id"
            ).AddParameter("review_id", reviewId);

            return connection.ExecuteScalar(query, value => value != null ? Convert.ToInt32(value) : 0);
        }

        public bool HasUserReportedReview(long reviewId, long reporterId)
        {
            using var connection = DatabaseConnection.Create();
            var query = DatabaseQuery.Create(
                "SELECT COUNT(*) FROM review_reports WHERE review_id = @review_id AND reporter_id = @reporter_id"
            ).AddParameter("review_id", reviewId)
             .AddParameter("reporter_id", reporterId);

            return connection.ExecuteScalar(query, value => value != null && Convert.ToInt32(value) > 0);
        }

        public ReviewReport Create(ReviewReport report)
        {
            using var connection = DatabaseConnection.Create();
            var query = DatabaseQuery.Create(
                @"INSERT INTO review_reports (review_id, reporter_id, created_at)
                VALUES (@review_id, @reporter_id, @created_at)
                RETURNING id"
            )
            .ExtendParameters(_reportMapper.MapToQueryParameters(report));

            long id = connection.ExecuteScalar(query, Convert.ToInt64);

            return report.CopyWith(id: id);
        }
    }
}
