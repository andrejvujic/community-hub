namespace CommunityHub.Application.Domain
{
    public class Country
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public Country(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public Country CopyWith(
            long? id = null,
            string? name = null
        )
        {
            return new Country(
                id ?? Id,
                name ?? Name
            );
        }
    }
}