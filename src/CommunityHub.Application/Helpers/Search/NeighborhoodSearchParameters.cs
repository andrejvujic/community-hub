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
    }
}
