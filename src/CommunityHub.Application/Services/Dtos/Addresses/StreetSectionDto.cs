using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Services.Dtos.Addresses
{
    public class StreetSectionDto
    {
        public long Id { get; set; }
        public int StartNumber { get; set; }
        public int EndNumber { get; set; }
        public ParityType Side { get; set; }
        public StreetDto Street { get; set; }

        public StreetSectionDto(
            long id,
            int startNumber,
            int endNumber,
            ParityType side,
            StreetDto street
        )
        {
            Id = id;
            StartNumber = startNumber;
            EndNumber = endNumber;
            Side = side;
            Street = street;
        }
    }
}