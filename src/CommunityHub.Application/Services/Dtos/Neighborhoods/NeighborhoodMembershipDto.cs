namespace CommunityHub.Application.Services.Dtos.Neighborhoods
{
    public class NeighborhoodMembershipDto
    {
        public long Id { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public NeighborhoodDto Neighborhood { get; set; }

        public NeighborhoodMembershipDto(
            long id,
            DateTime acceptanceDate,
            NeighborhoodDto neighborhood
        )
        {
            Id = id;
            AcceptanceDate = acceptanceDate;
            Neighborhood = neighborhood;
        }
    }
}
