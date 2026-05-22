namespace CommunityHub.Application.Helpers.Search
{

    public class BuildingSearchParameters
    {
        public long? CountryId { get; }
        public long? CityId { get; }
        public string StreetName { get; } = string.Empty;
        public string Neighborhood { get; } = string.Empty;

        public BuildingSearchParameters(
            long? countryId = null,
            long? cityId = null,
            string? streetName = null,
            string? neighborhood = null
        )
        {
            CountryId = countryId;
            CityId = cityId;
            StreetName = streetName ?? string.Empty;
            Neighborhood = neighborhood ?? string.Empty;
        }

        public static BuildingSearchParameters Empty() => new BuildingSearchParameters();

        public Dictionary<string, object?> ToQueryParameters()
        {
            return new Dictionary<string, object?>
            {
                { "country_id", CountryId },
                { "city_id", CityId },
                { "street_name", StreetName },
                { "neighborhood", Neighborhood }
            };
        }
    }
}
