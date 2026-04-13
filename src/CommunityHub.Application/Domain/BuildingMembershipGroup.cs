namespace CommunityHub.Application.Domain
{
    public class BuildingMembershipGroup
    {
        public User Resident;
        public Building Building { get; set; }
        public List<ApartmentMembership> Memberships { get; set; } = [];

        public BuildingMembershipGroup(Building building, User resident)
        {
            Resident = resident;
            Building = building;
        }
    }
}
