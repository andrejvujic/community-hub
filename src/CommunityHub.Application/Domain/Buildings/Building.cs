using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Domain.Buildings
{
    public class Building
    {
        public long Id { get;  private set; }
        public int FloorCount { get; private set; }
        public int? ApartmentCount { get; private set; }
        public User Manager {  get; private set; }

        public Address Address { get; private set; }
        public string DisplayAddress => $"{Address.Street.Name} {Address.StreetNumber}";

        public List<Image> Images { get; private set; } = new();
        public bool HasImages { get => Images.Count > 0; }
        public Image? CoverImage { get => HasImages ? Images.First() : null; }



        public Building(
            int floorCount,
            int? apartmentCount,
            User manager,
            Address address,
            long id = 0
        )
        {
            Id = id;
            FloorCount = floorCount;
            ApartmentCount = apartmentCount;
            Manager = manager;
            Address = address;
        }
        public Building CopyWith(
            long? id = null,
            int? floorCount = null,
            int? apartmentCount = null,
            User? manager = null,
            Address? address = null,
            List<Image>? images = null
        )
        {
            return new Building(
                floorCount ?? FloorCount,
                apartmentCount ?? ApartmentCount,
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