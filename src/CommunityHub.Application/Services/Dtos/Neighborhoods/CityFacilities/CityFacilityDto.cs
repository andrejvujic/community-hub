using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities
{
    public class CityFacilityDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CityDto City { get; set; }
        public CountryDto Country { get; set; }

        public CityFacilityDto(long id, string name, CityDto city, CountryDto country)
        {
            Id = id;
            Name = name;
            City = city;
            Country = country;
        }
    }
}
