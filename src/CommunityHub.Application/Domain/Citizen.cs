namespace CommunityHub.Application.Domain
{
    public class Citizen : User
    {
        public Address Address { get; private set; }
        public NeighborhoodMembership? Membership { get; private set; }

        public Citizen(
            long id,
            string username,
            string password,
            string name,
            string surname,
            Role role,
            Address address,
            NeighborhoodMembership? membership
        ) : base(id, username, password, name, surname, role)
        {
            Address = address;
            Membership = membership;
        }

        public Citizen CopyWith(
            long? id = null,
            string? username = null,
            string? password = null,
            string? name = null,
            string? surname = null,
            Role? role = null,
            Address? address = null,
            NeighborhoodMembership? membership = null
        )
        {
            return new Citizen(
                id ?? Id,
                username ?? Username,
                password ?? Password,
                name ?? Name,
                surname ?? Surname,
                role ?? Role,
                address ?? Address,
                membership ?? Membership
            );
        }

        public Citizen AssignMembership(NeighborhoodMembership membership)
        {
            return CopyWith(membership: membership);
        }
    }
}
