using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.Services.Dtos.Addresses
{
    public class StreetSectionDto
    {
        public long Id { get; set; }
        public int StartNumber { get; set; }
        public int EndNumber { get; set; }
        public ParityType Side { get; set; }
        public StreetDto Street { get; set; } = new StreetDto();
    }
}
