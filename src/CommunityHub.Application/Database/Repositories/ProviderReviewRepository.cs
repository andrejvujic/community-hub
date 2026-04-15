using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class ProviderReviewRepository
{
    private readonly ProviderReviewMapper _mapper = new ProviderReviewMapper();

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