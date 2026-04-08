namespace CommunityHub.Application.Helpers.Search
{

    public class CountrySearchParameters
    {
        public string Name { get; } = string.Empty;

        public CountrySearchParameters(string? name = null)
        {
            Name = name ?? string.Empty;
        }

        public Dictionary<string, object?> ToQueryParameters()
        {
            return new Dictionary<string, object?>
            {
                { "country_name", Name},
            };
        }
    }
}
