using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Database.Repositories.Shared;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories;

public class JobListingRepository
{
    private readonly JobListingMapper _mapper = new JobListingMapper();
    private readonly PriceEntryMapper _priceMapper = new PriceEntryMapper();
    private readonly ImageMapper _imageMapper = new ImageMapper();
    private readonly ImageRepository _imageRepository = new ImageRepository();

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
                lco.id AS listing_country_id,
                lco.name AS listing_country_name,
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
                pe.job_listing_id AS price_entry_listing_id,
                img.id AS image_id,
                img.path AS path,
                img.building_id AS image_building_id,
                img.neighborhood_id AS image_neighborhood_id,
                img.job_listing_id AS image_job_listing_id
            FROM job_listings jl
            INNER JOIN service_providers sp ON sp.id = jl.provider_id
            INNER JOIN users u ON u.id = sp.id
            INNER JOIN cities lc ON lc.id = jl.city_id
            INNER JOIN countries lco ON lco.id = lc.country_id
            INNER JOIN cities pc ON pc.id = sp.city_id
            INNER JOIN countries co ON co.id = sp.country_id
            LEFT JOIN price_entries pe ON pe.job_listing_id = jl.id
            LEFT JOIN images img ON img.job_listing_id = jl.id
            WHERE jl.provider_id = @provider_id
            ORDER BY jl.id, pe.valid_from, img.id"
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
            if (price is not null && !existing.PriceList.Any(p => p.Id == price.Id))
                existing.AddPriceEntry(price);

            var image = _imageMapper.MapToEntity(reader);
            if (image is not null && !existing.WorkSamples.Any(i => i.Id == image.Id))
                existing.AddWorkSample(image);
        });

        return dict.Values.ToList();
    }

    public JobListing Create(JobListing listing)
    {
        long newId = PostgresQuery.Create(
            @"INSERT INTO job_listings (service_name, description, billing_type, city_id, country_id, provider_id)
              VALUES (@service_name, @description, @billing_type, @city_id, @country_id, @provider_id)
              RETURNING id"
        )
        .ExtendParameters(_mapper.MapToQueryParameters(listing))
        .ExecuteScalar<long>(v => Convert.ToInt64(v));

        var savedPrices = new List<PriceEntry>();
        foreach (var entry in listing.PriceList)
        {
            var savedEntry = AddPriceEntry(entry.CopyWith(jobListingId: newId));
            savedPrices.Add(savedEntry);
        }

        var savedImages = new List<Image>();
        foreach (var image in listing.WorkSamples)
        {
            var savedImage = _imageRepository.Create(image);
            savedImages.Add(savedImage);
        }

        return listing.CopyWith(id: newId, priceList: savedPrices, workSamples: savedImages);
    }

    public PriceEntry AddPriceEntry(PriceEntry entry)
    {
        long newId = PostgresQuery.Create(
            @"INSERT INTO price_entries (amount, valid_from, valid_to, job_listing_id)
              VALUES (@amount, @valid_from, @valid_to, @job_listing_id)
              RETURNING id"
        )
        .ExtendParameters(_priceMapper.MapToQueryParameters(entry))
        .ExecuteScalar<long>(v => Convert.ToInt64(v));

        return entry.CopyWith(id: newId);
    }

    public List<JobListing> GetByCityAndCountry(long cityId, long countryId)
    {
        var query = PostgresQuery.Create(
            @"SELECT
            job_listing.id AS job_listing_id,
            job_listing.service_name,
            job_listing.description,
            job_listing.billing_type,

            listing_city.id AS listing_city_city_id,
            listing_city.name AS listing_city_city_name,

            listing_country.id AS listing_country_country_id,
            listing_country.name AS listing_country_country_name,

            users.id AS user_id,
            users.username,
            users.password,
            users.name,
            users.surname,
            users.user_role,

            provider_city.id AS provider_city_city_id,
            provider_city.name AS provider_city_city_name,

            provider_country.id AS provider_country_country_id,
            provider_country.name AS provider_country_country_name,

            price_entry.id AS price_entry_id,
            price_entry.amount,
            price_entry.valid_from,
            price_entry.valid_to,
            price_entry.job_listing_id AS price_entry_listing_id,

            image.id AS image_id,
            image.path AS path,
            image.building_id AS image_building_id,
            image.neighborhood_id AS image_neighborhood_id,
            image.job_listing_id AS image_job_listing_id

        FROM job_listings job_listing
        INNER JOIN service_providers service_provider ON service_provider.id = job_listing.provider_id
        INNER JOIN users ON users.id = service_provider.id
        INNER JOIN cities listing_city ON listing_city.id = job_listing.city_id
        INNER JOIN countries listing_country ON listing_country.id = job_listing.country_id
        INNER JOIN cities provider_city ON provider_city.id = service_provider.city_id
        INNER JOIN countries provider_country ON provider_country.id = service_provider.country_id
        LEFT JOIN price_entries price_entry ON price_entry.job_listing_id = job_listing.id
        LEFT JOIN images image ON image.job_listing_id = job_listing.id
        WHERE job_listing.city_id = @city_id AND job_listing.country_id = @country_id
        ORDER BY job_listing.id, price_entry.valid_from, image.id"
        )
        .AddParameter("city_id", cityId)
        .AddParameter("country_id", countryId);

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
            if (price is not null && !existing.PriceList.Any(p => p.Id == price.Id))
                existing.AddPriceEntry(price);

            var image = _imageMapper.MapToEntity(reader);
            if (image is not null && !existing.WorkSamples.Any(i => i.Id == image.Id))
                existing.AddWorkSample(image);
        });

        return dict.Values.ToList();
    }
}