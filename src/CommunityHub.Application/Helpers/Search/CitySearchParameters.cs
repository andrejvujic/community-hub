using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Helpers.Search
{

    public class CitySearchParameters
    {
        public Country? Country { get; set; } 
        public string Name { get; } = string.Empty;

        public CitySearchParameters(Country? country = null, string? name = null)
        {
            Country = country;
            Name = name ?? string.Empty;
        }

        public static CitySearchParameters Empty() => new CitySearchParameters();

        public Dictionary<string, object?> ToQueryParameters()
        {
            return new Dictionary<string, object?>
            {
                { "country_id", Country?.Id },
                { "city_name", Name},
            };
        }
    }
}
