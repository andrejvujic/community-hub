using CommunityHub.Application.Domain.CommonRooms.RentalRequests;

namespace CommunityHub.Application.Services.Dtos.CommonRooms
{
    public class CommonRoomDto
    {
        public long Id { get; set; }
        public long BuildingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FloorNumber { get; set; }
        public RentalType RentalType { get; set; }

        public CommonRoomDto(
            long id,
            long buildingId,
            string name,
            string description,
            int floorNumber,
            RentalType rentalType
        )
        {
            Id = id;
            BuildingId = buildingId;
            Name = name;
            Description = description;
            FloorNumber = floorNumber;
            RentalType = rentalType;
        }
    }
}