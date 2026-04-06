namespace CommunityHub.Application.Domain
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
    }
}
