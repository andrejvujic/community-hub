namespace CommunityHub.Application.Domain
{
    public class Building
    {
        public long Id { get;  private set; }
        public int FloorCount { get; private set; }
        public User Manager {  get; private set; }
        public Address Address { get; private set; }
        public List<Image> Images { get; private set; } = new List<Image>();

        public Building(
            long id,
            int floorCount,
            User manager,
            Address address

        )
        {
            Id = id;
            FloorCount = floorCount;
            Manager = manager;
            Address = address;
        }

        public Building(
            int floorCount,
            User manager,
            Address address
        )
        {
            FloorCount = floorCount;
            Manager = manager;
            Address = address;
        }
    }
}
