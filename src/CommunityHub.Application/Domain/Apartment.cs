namespace CommunityHub.Application.Domain
{
    public class Apartment
    {
        public long Id { get; private set; }
        public int Number { get; private set; }
        public int FloorNumber { get; private set; }

        public Apartment(long id, int number, int floorNumber)
        {
            Id = id;
            Number = number;
            FloorNumber = floorNumber;
        }
    }
}
