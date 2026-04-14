using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{

    public class JobListingMapper : EntityMapper<JobListing>
    {
        private readonly ServiceProviderMapper _providerMapper = new ServiceProviderMapper();
        private readonly PriceEntryMapper _priceMapper = new PriceEntryMapper();
        private readonly WorkSampleMapper _sampleMapper = new WorkSampleMapper();

        public override JobListing? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("job_listing_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            ServiceProvider? provider = _providerMapper.MapToEntity(reader);
            if (provider is null) return null;

            long listingCityId = reader.GetInt64(reader.GetOrdinal("listing_city_id"));
            string listingCityName = reader.GetString(reader.GetOrdinal("listing_city_name"));
            City listingCity = new City(listingCityId, listingCityName);

            ServiceName serviceName = ServiceName.FromValue(
                reader.GetString(reader.GetOrdinal("service_name")));
            string description = reader.GetString(reader.GetOrdinal("description"));
            BillingType billingType = (BillingType)reader.GetValue(reader.GetOrdinal("billing_type"));

            var priceEntries = new List<PriceEntry>();
            var workSamples = new List<WorkSample>();

            if (reader.HasColumn("price_entry_id"))
            {
                PriceEntry? entry = _priceMapper.MapToEntity(reader);
                if (entry is not null) priceEntries.Add(entry);
            }

            if (reader.HasColumn("work_sample_id"))
            {
                WorkSample? sample = _sampleMapper.MapToEntity(reader);
                if (sample is not null) workSamples.Add(sample);
            }

            return new JobListing(
                serviceName,
                description,
                billingType,
                listingCity,
                provider,
                id: id,
                priceList: priceEntries,
                workSamples: workSamples
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(JobListing entity)
        {
            return new Dictionary<string, object?>
            {
                { "service_name", entity.ServiceName.Value },
                { "description", entity.Description },
                { "billing_type", entity.BillingType },
                { "city_id", entity.City.Id },
                { "provider_id", entity.Provider.Id }
            };
        }
    }
}