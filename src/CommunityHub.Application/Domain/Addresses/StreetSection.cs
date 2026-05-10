namespace CommunityHub.Application.Domain.Addresses
{
    public class StreetSection
    {
        public long Id { get; private set; }
        public int StartNumber { get; private set; }
        public int EndNumber { get; private set; }
        public ParityType Side { get; private set; }
        public Street Street { get; private set; }

        public StreetSection(int startNumber, int endNumber, ParityType side, Street street, long id = 0)
        {
            Id = id;
            StartNumber = startNumber;
            EndNumber = endNumber;
            Side = side;
            Street = street;
        }

        public StreetSection CopyWith(
            long? id = null,
            int? startNumber = null,
            int? endNumber = null,
            ParityType? side = null,
            Street? street = null
        )
        {
            return new StreetSection(
                startNumber ?? StartNumber,
                endNumber ?? EndNumber,
                side ?? Side,
                street ?? Street,
                id ?? Id
            );
        }
    }
}