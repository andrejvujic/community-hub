using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class JobListingRepository
{
    private readonly JobListingMapper _mapper = new JobListingMapper();
    private readonly PriceEntryMapper _priceMapper = new PriceEntryMapper();

    public List<JobListing> GetByProviderId(long providerId)
    {
        var query = PostgresQuery.Create(
            @"SELECT
                jl.id AS job_listing_id,
                jl.service_name,
                jl.description,
                jl.billing_type,
                lc.id AS listing_city_id,
                lc.name AS listing_city_name,
                u.id AS user_id,
                u.username,
                u.password,
                u.name,
                u.surname,
                u.user_role,
                pc.id AS provider_city_id,
                pc.name AS provider_city_name,
                co.id AS provider_country_id,
                co.name AS provider_country_name,
                pe.id AS price_entry_id,
                pe.amount,
                pe.valid_from,
                pe.valid_to,
                pe.job_listing_id AS price_entry_listing_id
            FROM job_listings jl
            INNER JOIN service_providers sp ON sp.id = jl.provider_id
            INNER JOIN users u ON u.id  = sp.id
            INNER JOIN cities lc ON lc.id = jl.city_id
            INNER JOIN cities pc ON pc.id = sp.city_id
            INNER JOIN countries co ON co.id = sp.country_id
            LEFT  JOIN price_entries pe ON pe.job_listing_id = jl.id
            WHERE jl.provider_id = @provider_id
            ORDER BY jl.id, pe.valid_from"
        ).AddParameter("provider_id", providerId);

        var dict = new Dictionary<long, JobListing>();

        query.ExecuteReader(reader =>
        {
            var listing = _mapper.MapToEntity(reader);
            if (listing is null) return;

            if (!dict.TryGetValue(listing.Id, out var existing))
            {
                dict[listing.Id] = listing;
                existing = listing;
            }

            var price = _priceMapper.MapToEntity(reader);
            if (price is not null &&
                !existing.PriceList.Any(p => p.Id == price.Id))
            {
                existing.PriceList.Add(price);
            }
        });

        return dict.Values.ToList();
    }

    public JobListing Create(JobListing listing)
    {
        var query = PostgresQuery.Create(
            @"INSERT INTO job_listings (service_name, description, billing_type, city_id, provider_id)
              VALUES (@service_name, @description, @billing_type, @city_id, @provider_id)
              RETURNING id"
        ).ExtendParameters(_mapper.MapToQueryParameters(listing));

        long newId = query.ExecuteScalar<long>(v => Convert.ToInt64(v));
        return listing.CopyWith(id: newId);
    }

    public PriceEntry AddPriceEntry(PriceEntry entry)
    {
        var query = PostgresQuery.Create(
            @"INSERT INTO price_entries (amount, valid_from, valid_to, job_listing_id)
              VALUES (@amount, @valid_from, @valid_to, @job_listing_id)
              RETURNING id"
        ).ExtendParameters(_priceMapper.MapToQueryParameters(entry));

        long newId = query.ExecuteScalar<long>(v => Convert.ToInt64(v));
        return entry.CopyWith(id: newId);
    }
}