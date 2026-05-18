using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.Domain.Neighborhoods.CityFacilities
{
    public class CityFacility
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public City City { get; private set; }
        public Country Country { get; private set; }

        public CityFacility(
            string name,
            City city,
            Country country,
            long id = 0
        )
        {
            Id = id;
            Name = name;
            City = city;
            Country = country;
        }

        public CityFacility CopyWith(
            long? id = null,
            string? name = null,
            City? city = null,
            Country? country = null
        )
        {
            return new CityFacility(
                name ?? Name,
                city ?? City,
                country ?? Country,
                id ?? Id
            );
        }
    }
}