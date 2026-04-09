namespace CommunityHub.Application.Domain
{
    public class Building
    {
        public long Id { get;  private set; }
        public int FloorCount { get; private set; }
        public User Manager {  get; private set; }
        public Address Address { get; private set; }

        public List<Image> Images { get; private set; } = new List<Image>();
        public bool HasImages { get => Images.Count > 0; }
        public Image? CoverImage { get => HasImages ? Images.First() : null; }

        public Building(
            int floorCount,
            User manager,
            Address address,
            long id = 0
        )
        {
            Id = id;
            FloorCount = floorCount;
            Manager = manager;
            Address = address;
        }

        public Building CopyWith(
            long? id = null,
            int? floorCount = null,
            User? manager = null,
            Address? address = null,
            List<Image>? images = null
        )
        {
            return new Building(
                floorCount ?? FloorCount,
                manager ?? Manager,
                address ?? Address,
                id ?? Id
            )
            {
                Images = images ?? Images
            };
        }
    }
}
