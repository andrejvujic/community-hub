namespace CommunityHub.Application.Domain
{
    public class ServiceProvider : User
    {
        public City City { get; private set; }

        public ServiceProvider(
            long id,
            string username,
            string password,
            string name,
            string surname,
            Role role,
            City city
        ) : base(id, username, password, name, surname, role)
        {
            City = city;
        }

        public ServiceProvider CopyWith(
            long? id = null,
            string? username = null,
            string? password = null,
            string? name = null,
            string? surname = null,
            Role? role = null,
            City? city = null
        )
        {
            return new ServiceProvider(
                id ?? Id,
                username ?? Username,
                password ?? Password,
                name ?? Name,
                surname ?? Surname,
                role ?? Role,
                city ?? City
            );
        }
    }
}