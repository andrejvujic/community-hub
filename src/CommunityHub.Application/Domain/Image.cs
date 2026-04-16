namespace CommunityHub.Application.Domain
{
    public class Image
    {
        public long Id { get; private set; }
        public long? BuildingId { get; private set; }
        public long? NeighborhoodId { get; private set; }
        public string Path { get; private set; }

        public Image(long id, string path, long? buildingId = null, long? neighborhoodId = null)
        {
            if ((buildingId != null && neighborhoodId != null) || (buildingId == null && neighborhoodId == null))
                throw new ArgumentException("An image must be associated with either a building or a neighborhood.");

            Id = id;
            Path = path;
            BuildingId = buildingId;
            NeighborhoodId = neighborhoodId;
        }
        public Image CopyWith(
            long? id = null,
            string? path = null,
            long? buildingId = null,
            long? neighborhoodId = null
        )
        {
            return new Image(
                id ?? Id,
                path ?? Path,
                buildingId ?? BuildingId,
                neighborhoodId ?? NeighborhoodId
            );
        }
    }
}