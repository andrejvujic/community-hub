using CommunityHub.Application.Services.Dtos.Buildings;

namespace CommunityHub.Application.Services.Dtos.Apartments
{
    public class ApartmentDto
    {
        public long Id { get; set; }
        public int Number { get; set; }
        public int FloorNumber { get; set; }
        public BuildingDto Building { get; set; }

        public ApartmentDto(long id, int number, int floorNumber, BuildingDto building)
        {
            Id = id;
            Number = number;
            FloorNumber = floorNumber;
            Building = building;
        }
    }
}