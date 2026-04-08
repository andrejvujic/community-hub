using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Helpers.Search
{

    public class BuildingSearchParameters
    {
        public Country? Country { get; }
        public City? City { get; }
        public string StreetText { get; } = string.Empty;
        public string NeighborhoodText { get; } = string.Empty;

        public BuildingSearchParameters(
            Country? country = null,
            City? city = null,
            string? streetText = null,
            string? neighborhoodText = null
        )
        {
            Country = country;
            City = city;
            StreetText = streetText ?? string.Empty;
            NeighborhoodText = neighborhoodText ?? string.Empty;
        }

        public static BuildingSearchParameters Default() => new BuildingSearchParameters();

        public Dictionary<string, object?> ToQueryParameters()
        {
            return new Dictionary<string, object?>
            {
                { "country_id", Country == null ? 0 : Country.Id },
                { "city_id", City == null ? 0 : City.Id },
                { "street_text", StreetText },
                { "neighborhood_text", NeighborhoodText }
            };
        }
    }
}
