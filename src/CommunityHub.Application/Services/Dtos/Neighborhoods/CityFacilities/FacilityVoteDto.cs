using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities
{
    public class FacilityVoteDto
    {
        public long Id { get; set; }
        public CityFacilityDto Facility { get; set; }
        public CitizenDto Citizen { get; set; }

        public FacilityVoteDto(
            long id,
            CityFacilityDto facility,
            CitizenDto citizen
        )
        {
            Id = id;
            Facility = facility;
            Citizen = citizen;
        }
    }
}
