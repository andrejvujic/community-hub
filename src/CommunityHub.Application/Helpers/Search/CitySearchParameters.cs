namespace CommunityHub.Application.Helpers.Search
{

    public class CitySearchParameters
    {
        public string Name { get; } = string.Empty;

        public CitySearchParameters(string? name = null)
        {
            Name = name ?? string.Empty;
        }

        public static CitySearchParameters Empty() => new CitySearchParameters();

        public Dictionary<string, object?> ToQueryParameters()
        {
            return new Dictionary<string, object?>
            {
                { "city_name", Name},
            };
        }
    }
}
