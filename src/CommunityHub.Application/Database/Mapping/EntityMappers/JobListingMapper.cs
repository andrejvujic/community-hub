using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Addresses;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class JobListingMapper : EntityMapper<JobListing>
    {
        private readonly ServiceProviderMapper _providerMapper = new ServiceProviderMapper();
        private readonly CityMapper _cityMapper = new CityMapper();
        private readonly CountryMapper _countryMapper = new CountryMapper();

        public override JobListing? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("job_listing_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            ServiceProvider? provider = _providerMapper.MapToEntity(reader);
            if (provider is null) return null;

            City listingCity = _cityMapper.MapToEntity(reader.WithPrefix("listing_city"));
            Country listingCountry = _countryMapper.MapToEntity(reader.WithPrefix("listing_country"));

            string serviceName = reader.GetString(reader.GetOrdinal("service_name"));
            string description = reader.GetString(reader.GetOrdinal("description"));
            BillingType billingType = (BillingType)reader.GetValue(reader.GetOrdinal("billing_type"));

            return new JobListing(
                serviceName,
                description,
                billingType,
                listingCity,
                listingCountry,
                provider,
                id: id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(JobListing entity)
        {
            return new Dictionary<string, object?>
            {
                { "service_name", entity.ServiceName },
                { "description", entity.Description },
                { "billing_type", entity.BillingType },
                { "city_id", entity.City.Id },
                { "country_id", entity.Country.Id },
                { "provider_id", entity.Provider.Id }
            };
        }
    }
}