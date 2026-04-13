namespace CommunityHub.Application.Domain
{
    public class BuildingMembershipGroup(Building building, User resident)
    {
        public User Resident = resident;
        public Building Building { get; set; } = building;
        public List<ApartmentMembership> Memberships { get; set; } = [];
    }
}
