namespace CommunityHub.Application.Services.Dtos.Addresses
{
    public class AddressDto
    {
        public long Id { get; set; }
        public StreetDto Street { get; set; }
        public string StreetNumber { get; set; }
        public string Neighborhood { get; set; }
        public CityDto City { get; set; }
        public CountryDto Country { get; set; }

        public AddressDto(
            long id,
            StreetDto street,
            string streetNumber,
            string neighborhood,
            CityDto city,
            CountryDto country
        )
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