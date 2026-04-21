namespace CommunityHub.Application.Domain.CommonRoom
{
    public class CommonRoom
    {
        public long Id { get; private set; }
        public long BuildingId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int FloorNumber { get; private set; }
        public RentalType Type { get; private set; }

        public CommonRoom(
            long buildingId,
            string name,
            string description,
            int floorNumber,
            RentalType type,
            long id = 0
        ) {
            BuildingId = buildingId;
            Name = name;
            Description = description;
            FloorNumber = floorNumber;

            if (floorNumber < 1) throw new ArgumentOutOfRangeException("Floor number must be greater than 0.");

            Type = type;
        }

        public CommonRoom CopyWith(
            long? buildingId = null,
            string? name = null,
            string? description = null,
            int? floorNumber = null,
            RentalType? type = null
        )
        {
            return new CommonRoom(
                buildingId ?? BuildingId,
                name ?? Name,
                description ?? Description,
                floorNumber ?? FloorNumber,
                type ?? Type
            );
        }
    }
}