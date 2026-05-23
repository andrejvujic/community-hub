using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Buildings
{
    public class BuildingMembershipGroupDto
    {
        public UserDto Resident;
        public BuildingDto Building { get; set; }
        public List<ApartmentMembershipDto> Memberships { get; set; } = new();

        public BuildingMembershipGroupDto(BuildingDto building, UserDto resident)
        {
            Resident = resident;
            Building = building;
        }
    }
}
