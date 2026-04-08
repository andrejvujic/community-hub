namespace CommunityHub.Application.Domain
{
    public class NeighborhoodMembership
    {
        public long Id { get; private set; }
        public DateTime AcceptanceDate { get; private set; }
        public Neighborhood Neighborhood { get; private set; }

        public NeighborhoodMembership(long id, DateTime acceptanceDate, Neighborhood neighborhood)
        {
            Id = id;
            AcceptanceDate = acceptanceDate;
            Neighborhood = neighborhood;
        }

        public NeighborhoodMembership CopyWith(
            long? id = null,
            DateTime? acceptanceDate = null,
            Neighborhood? neighborhood = null
        )
        {
            return new NeighborhoodMembership(
                id ?? Id,
                acceptanceDate ?? AcceptanceDate,
                neighborhood ?? Neighborhood
            );
        }
    }
}