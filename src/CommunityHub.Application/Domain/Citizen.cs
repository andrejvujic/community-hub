namespace CommunityHub.Application.Domain
{
    public class Citizen : User
    {
        public Address Address { get; private set; }
        public NeighborhoodMembership Membership { get; private set; }

        public Citizen(
            long id,
            string username,
            string password,
            string name,
            string surname,
            Role role,
            Address address,
            NeighborhoodMembership membership
        ) : base(id, username, password, name, surname, role)
        {
            Address = address;
            Membership = membership;
        }
    }
}
