using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Helpers.Search
{

    public class BuildingSearchParameters
    {
        public Country? Country { get; }
        public City? City { get; }
        public string StreetName { get; } = string.Empty;
        public string Neighborhood { get; } = string.Empty;

        public BuildingSearchParameters(
            Country? country = null,
            City? city = null,
            string? streetName = null,
            string? neighborhood = null
        )
        {
            Country = country;
            City = city;
            StreetName = streetName ?? string.Empty;
            Neighborhood = neighborhood ?? string.Empty;
        }

        public static BuildingSearchParameters Empty() => new BuildingSearchParameters();

        public Dictionary<string, object?> ToQueryParameters()
        {
            return new Dictionary<string, object?>
            {
                { "country_id", Country?.Id },
                { "city_id", City?.Id },
                { "street_name", StreetName },
                { "neighborhood", Neighborhood }
            };
        }
    }
}
