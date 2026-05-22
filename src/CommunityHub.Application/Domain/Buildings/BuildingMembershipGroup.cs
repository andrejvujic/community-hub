using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Domain.Buildings
{
    public class BuildingMembershipGroup
    {
        public UserDto Resident;
        public BuildingDto Building { get; set; }
        public List<ApartmentMembershipDto> Memberships { get; set; } = [];

        public BuildingMembershipGroup(BuildingDto building, UserDto resident)
        {
            Resident = resident;
            Building = building;
        }
    }
}
