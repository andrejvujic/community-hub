using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities
{
    public class CityFacilityDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CityDto City { get; set; }
        public CountryDto Country { get; set; }
        public int VoteCount { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public bool VotedByMe { get; set; }

        public string LastVisitDateDisplay => LastVisitDate?.ToString("d") ?? "-";

        public CityFacilityDto(long id, string name, CityDto city, CountryDto country)
        {
            Id = id;
            Name = name;
            City = city;
            Country = country;
            VoteCount = 0;
            LastVisitDate = null;
            VotedByMe = false;
        }
    }
}
