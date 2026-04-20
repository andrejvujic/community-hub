namespace CommunityHub.Application.Domain
{
    public class Image
    {
        public long Id { get; private set; }
        public string Path { get; private set; }
        public long? BuildingId { get; private set; }
        public long? NeighborhoodId { get; private set; }
        public long? JobListingId { get; private set; }

        private Image(
            long id,
            string path,
            long? buildingId = null,
            long? neighborhoodId = null,
            long? jobListingId = null
        )
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Invalid image path. Path can't be empty.");

            Id = id;
            Path = path;
            BuildingId = buildingId;
            NeighborhoodId = neighborhoodId;
            JobListingId = jobListingId;
        }

        public static Image ForBuilding(long id, string path, long buildingId) =>
            new Image(id, path, buildingId: buildingId);

        public static Image ForNeighborhood(long id, string path, long neighborhoodId) =>
            new Image(id, path, neighborhoodId: neighborhoodId);

        public static Image ForJobListing(long id, string path, long jobListingId) =>
            new Image(id, path, jobListingId: jobListingId);

        public Image CopyWith(
            long? id = null,
            string? path = null
        )
        {
            return new Image(
                id ?? Id,
                path ?? Path,
                buildingId: BuildingId,
                neighborhoodId: NeighborhoodId,
                jobListingId: JobListingId
            );
        }
    }
}