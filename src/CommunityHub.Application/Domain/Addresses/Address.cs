namespace CommunityHub.Application.Domain.Addresses
{
    public class Address
    {
        public long Id { get; private set; }
        public Street Street { get; private set; }
        public string StreetNumber { get; private set; }
        public string Neighborhood { get; private set; }
        public City City { get; private set; }
        public Country Country { get; private set; }

        public Address(long id, Street street, string streetNumber, string neighborhood, City city, Country country)
        {
            Id = id;
            Street = street;
            StreetNumber = streetNumber;
            Neighborhood = neighborhood;
            City = city;
            Country = country;
        }
        public Address CopyWith(
            long? id = null,
            Street? street = null,
            string? streetNumber = null,
            string? neighborhood = null,
            City? city = null,
            Country? country = null
        )
        {
            return new Address(
                id ?? Id,
                street ?? Street,
                streetNumber ?? StreetNumber,
                neighborhood ?? Neighborhood,
                city ?? City,
                country ?? Country
            );
        }
    }
}