using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class ProviderReviewRepository
{
    private readonly ProviderReviewMapper _mapper = new ProviderReviewMapper();

    private const string SelectColumns = @"
        pr.id AS review_id,
        pr.quality_rating,
        pr.communication_rating,
        pr.comment AS review_comment,
        pr.created_at AS review_created_at,
        pr.updated_at,
        bj.id AS booked_job_id,
        bj.scheduled_date,
        bj.scheduled_time,
        bj.duration_hours,
        bj.unit_price,
        bj.is_substitution,
        jl.id AS job_listing_id,
        jl.service_name,
        jl.description,
        jl.billing_type,
        lc.id AS listing_city_id,
        lc.name AS listing_city_name,
        lco.id AS listing_country_id,
        lco.name AS listing_country_name,
        pu.id AS user_id,
        pu.username,
        pu.password,
        pu.name,
        pu.surname,
        pu.user_role,
        pc.id AS provider_city_id,
        pc.name AS provider_city_name,
        pco.id AS provider_country_id,
        pco.name AS provider_country_name,
        n.id AS neighborhood_id,
        n.name AS neighborhood_name,
        n.description,
        n.budget,
        nc.id AS city_id,
        nc.name AS city_name,
        nco.id AS country_id,
        nco.name AS country_name,
        cu.id AS coordinator_user_id,
        cu.username AS coordinator_username,
        cu.password AS coordinator_password,
        cu.name AS coordinator_name,
        cu.surname AS coordinator_surname,
        cu.user_role AS coordinator_user_role,
        sub_u.id AS sub_provider_user_id,
        sub_u.username AS sub_provider_username,
        sub_u.password AS sub_provider_password,
        sub_u.name AS sub_provider_name,
        sub_u.surname AS sub_provider_surname,
        sub_u.user_role AS sub_provider_user_role,
        sub_c.id AS sub_provider_city_id,
        sub_c.name AS sub_provider_city_name,
        sub_co.id AS sub_provider_country_id,
        sub_co.name AS sub_provider_country_name";

    private const string FromJoins = @"
        FROM provider_reviews pr
        INNER JOIN booked_jobs bj ON bj.id = pr.booked_job_id
        INNER JOIN job_listings jl ON jl.id = bj.job_listing_id
        INNER JOIN service_providers sp ON sp.id = jl.provider_id
        INNER JOIN users pu ON pu.id = sp.id
        INNER JOIN cities lc ON lc.id = jl.city_id
        INNER JOIN countries lco ON lco.id = lc.country_id
        INNER JOIN cities pc ON pc.id = sp.city_id
        INNER JOIN countries pco ON pco.id = sp.country_id
        INNER JOIN neighborhoods n ON n.id = bj.neighborhood_id
        INNER JOIN cities nc ON nc.id = n.city_id
        INNER JOIN countries nco ON nco.id = nc.country_id
        INNER JOIN users cu ON cu.id = n.coordinator_id
        INNER JOIN users rev_u ON rev_u.id = pr.coordinator_id
        LEFT JOIN service_providers sub_sp ON sub_sp.id = bj.substituting_provider_id
        LEFT JOIN users sub_u ON sub_u.id = sub_sp.id
        LEFT JOIN cities sub_c ON sub_c.id = sub_sp.city_id
        LEFT JOIN countries sub_co ON sub_co.id = sub_sp.country_id";

    public List<ProviderReview> GetByProviderId(long providerId)
    {
        var query = PostgresQuery.Create(
            $"SELECT {SelectColumns} {FromJoins} WHERE jl.provider_id = @provider_id ORDER BY pr.created_at DESC"
        ).AddParameter("provider_id", providerId);

        var reviews = new List<ProviderReview>();
        query.ExecuteReader(reader =>
        {
            var review = _mapper.MapToEntity(reader);
            if (review is not null) reviews.Add(review);
        });
        return reviews;
    }

    public ProviderReview? GetByBookedJobId(long bookedJobId)
    {
        var query = PostgresQuery.Create(
            $"SELECT {SelectColumns} {FromJoins} WHERE pr.booked_job_id = @booked_job_id"
        ).AddParameter("booked_job_id", bookedJobId);

        ProviderReview? result = null;
        query.ExecuteReader(reader => result = _mapper.MapToEntity(reader));
        return result;
    }

    public ProviderReview Create(ProviderReview review)
    {
        long newId = PostgresQuery.Create(
            @"INSERT INTO provider_reviews
                (quality_rating, communication_rating, comment, created_at, booked_job_id, coordinator_id)
              VALUES
                (@quality_rating, @communication_rating, @comment, @created_at, @booked_job_id, @coordinator_id)
              RETURNING id"
        )
        .ExtendParameters(_mapper.MapToQueryParameters(review))
        .AddParameter("created_at", review.CreatedAt)
        .ExecuteScalar<long>(v => Convert.ToInt64(v));

        return review.CopyWith(id: newId);
    }

    public ProviderReview Update(ProviderReview review)
    {
        DateTime updatedAt = DateTime.UtcNow;

        PostgresQuery.Create(
            @"UPDATE provider_reviews
              SET quality_rating = @quality_rating,
                  communication_rating = @communication_rating,
                  comment = @comment,
                  updated_at = @updated_at
              WHERE id = @id"
        )
        .AddParameter("id", review.Id)
        .AddParameter("quality_rating", review.QualityRating)
        .AddParameter("communication_rating", review.CommunicationRating)
        .AddParameter("comment", review.Comment)
        .AddParameter("updated_at", updatedAt)
        .GetRowsAffectedCount();

        return review.CopyWith(updatedAt: updatedAt);
    }

    public decimal? GetAverageRatingForProvider(long providerId)
    {
        var query = PostgresQuery.Create(
            @"SELECT AVG((pr.quality_rating + pr.communication_rating) / 2.0)
              FROM provider_reviews pr
              INNER JOIN booked_jobs bj ON bj.id = pr.booked_job_id
              INNER JOIN job_listings jl ON jl.id = bj.job_listing_id
              WHERE jl.provider_id = @provider_id"
        ).AddParameter("provider_id", providerId);

        return query.ExecuteScalar<decimal?>(value =>
        {
            if (value == null || value == DBNull.Value) return null;
            return Convert.ToDecimal(value);
        });
    }
}