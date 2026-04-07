namespace CommunityHub.Application.Domain
{
    public class Apartment
    {
        public long Id { get; private set; }
        public int Number { get; private set; }
        public int FloorNumber { get; private set; }
        public Building Building { get; private set; }

        public Apartment(long id, int number, int floorNumber, Building building)
        {
            Id = id;
            Number = number;
            FloorNumber = floorNumber;
            Building = building;
        }
    }
}
