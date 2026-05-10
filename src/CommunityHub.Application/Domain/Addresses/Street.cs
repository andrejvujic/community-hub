namespace CommunityHub.Application.Domain.Addresses
{
    public class Street
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public Street(long id, string name)
        {     
            Id = id;
            Name = name;
        }

        public Street CopyWith(
            long? id = null,
            string? name = null
        )
        {
            return new Street(
                id ?? Id,
                name ?? Name
            );
        }
    }
}