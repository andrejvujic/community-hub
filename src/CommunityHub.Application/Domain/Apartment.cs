namespace CommunityHub.Application.Domain
{
    public class Apartment
    {
        public long Id { get; private set; }
        public int Number { get; private set; }
        public int Floor { get; private set; }

        public Apartment(long id, int number, int floor)
        {
            Id = id;
            Number = number;
            Floor = floor;
        }
    }
}
