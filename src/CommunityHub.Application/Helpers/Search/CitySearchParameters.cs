using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.Helpers.Search
{

    public class CitySearchParameters
    {
        public long? CountryId { get; set; } 
        public string Name { get; } = string.Empty;

        public CitySearchParameters(long? countryId = null, string? name = null)
        {
            CountryId = countryId;
            Name = name ?? string.Empty;
        }

        public static CitySearchParameters Empty() => new CitySearchParameters();

        public Dictionary<string, object?> ToQueryParameters()
        {
            return new Dictionary<string, object?>
            {
                { "country_id", CountryId },
                { "city_name", Name},
            };
        }
    }
}
