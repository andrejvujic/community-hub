namespace CommunityHub.Application.Domain
{
    public class Appartment
    {
        public long Id { get; private set; }
        public int Number { get; private set; }
        public int Floor { get; private set; }

        public Appartment(long id, int number, int floor)
        {
            Id = id;
            Number = number;
            Floor = floor;
        }
    }
}
