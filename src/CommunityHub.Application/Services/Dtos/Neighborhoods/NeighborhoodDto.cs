using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Images;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods
{
    public class NeighborhoodDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Budget { get; set; }
        public CityDto City { get; set; }
        public CountryDto Country { get; set; }
        public UserDto Coordinator { get; set; }
        public List<StreetSectionDto> StreetSections { get; set; }
        public List<ImageDto> Images { get; set; }

        public NeighborhoodDto(
            long id,
            string name,
            string description,
            int budget,
            CityDto city,
            CountryDto country,
            UserDto coordinator,
            List<StreetSectionDto>? streetSections = null,
            List<ImageDto>? images = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Budget = budget;
            City = city;
            Country = country;
            Coordinator = coordinator;
            StreetSections = streetSections ?? new List<StreetSectionDto>();
            Images = images ?? new List<ImageDto>();
        }
    }
}
