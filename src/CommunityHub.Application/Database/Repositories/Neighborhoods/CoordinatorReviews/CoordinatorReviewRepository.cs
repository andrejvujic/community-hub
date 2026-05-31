using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Neighborhoods.CoordinatorReviews;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CoordinatorReviews;

namespace CommunityHub.Application.Database.Repositories.Neighborhoods.CoordinatorReviews
{
    public class CoordinatorReviewRepository : ICoordinatorReviewRepository
    {
        private readonly CoordinatorReviewMapper _reviewMapper = new CoordinatorReviewMapper();

        private const string SelectColumns = @"
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

            address.id AS address_id,
            address.street_number,
            address.neighborhood,

            street.id AS street_id,
            street.name AS street_name,

            city.id AS city_id,
            city.name AS city_name,

            country.id AS country_id,
            country.name AS country_name,

            membership.id AS membership_id,
            membership.acceptance_date";

        private const string FromJoins = @"
            FROM coordinator_reviews coordinator_review
            INNER JOIN neighborhoods neighborhood ON neighborhood.id = coordinator_review.neighborhood_id
            INNER JOIN cities neighborhood_city ON neighborhood_city.id = neighborhood.city_id
            INNER JOIN countries neighborhood_country ON neighborhood_country.id = neighborhood_city.country_id
            INNER JOIN users coordinator ON coordinator.id = neighborhood.coordinator_id
            INNER JOIN citizens reviewer ON reviewer.id = coordinator_review.reviewer_id
            INNER JOIN addresses address ON reviewer.address_id = address.id
            INNER JOIN streets street ON address.street_id = street.id
            INNER JOIN cities city ON address.city_id = city.id
            INNER JOIN countries country ON city.country_id = country.id
            LEFT JOIN neighborhood_memberships membership ON reviewer.membership_id = membership.id";

        public CoordinatorReview? GetById(long id)
        {
            var query = PostgresQuery.Create(
                $"SELECT {SelectColumns} {FromJoins} WHERE coordinator_review.id = @id"
            ).AddParameter("id", id);

            CoordinatorReview? review = null;
            query.ExecuteReader(reader =>
            {
                review = _reviewMapper.MapToEntity(reader);
            });
            return review;
        }

        public List<CoordinatorReview> GetByNeighborhoodId(long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                $"SELECT {SelectColumns} {FromJoins} WHERE coordinator_review.neighborhood_id = @neighborhood_id AND coordinator_review.is_removed = false ORDER BY coordinator_review.created_at DESC"
            ).AddParameter("neighborhood_id", neighborhoodId);

            List<CoordinatorReview> reviews = new List<CoordinatorReview>();
            query.ExecuteReader(reader =>
            {
                var review = _reviewMapper.MapToEntity(reader);
                if (review is not null) reviews.Add(review);
            });
            return reviews;
        }

        public CoordinatorReview Create(CoordinatorReview review)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO coordinator_reviews (rating, comment, created_at, neighborhood_id, reviewer_id, is_removed)
                VALUES (@rating, @comment, @created_at, @neighborhood_id, @reviewer_id, @is_removed)
                RETURNING id"
            )
            .ExtendParameters(_reviewMapper.MapToQueryParameters(review));

            long id = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return review.CopyWith(id: id);
        }

        public void MarkAsRemoved(long reviewId)
        {
            var query = PostgresQuery.Create(
                "UPDATE coordinator_reviews SET is_removed = true WHERE id = @id"
            ).AddParameter("id", reviewId);

            query.GetRowsAffectedCount();
        }
    }
}
