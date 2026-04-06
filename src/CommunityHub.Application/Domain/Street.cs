namespace CommunityHub.Application.Domain
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
    }
}
