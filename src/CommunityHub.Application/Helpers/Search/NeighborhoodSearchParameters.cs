using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Helpers.Search
{
    public class NeighborhoodSearchParameters
    {
        public Country? Country { get; }
        public City? City { get; }
        public string Name { get; } = string.Empty;
        public string Address { get; } = string.Empty;

        public NeighborhoodSearchParameters(
            Country? country = null,
            City? city = null,
            string? name = null,
            string? address = null
        )
        {
            Country = country;
            City = city;
            Name = name ?? string.Empty;
            Address = address ?? string.Empty;
        }

        public static NeighborhoodSearchParameters Empty() => new NeighborhoodSearchParameters();

        public Dictionary<string, object?> ToQueryParameters()
        {
            var (streetName, streetNumber) = ParseAddress();

            return new Dictionary<string, object?>
            {
                { "country_id", Country?.Id },
                { "city_id", City?.Id },
                { "name", Name },
                { "street_name", streetName },
                { "street_number", streetNumber }
            };
        }

        private (string? StreetName, int? StreetNumber) ParseAddress()
        {
            if (string.IsNullOrWhiteSpace(Address))
                return (null, null);

            var parts = Address.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 1 && int.TryParse(parts[^1], out int parsedNumber))
            {
                return (string.Join(" ", parts[..^1]), parsedNumber);
            }

            return (Address.Trim(), null);
        }
    }
}
