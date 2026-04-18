namespace CommunityHub.Application.Domain
{
    public class Image
    {
        public long Id { get; private set; }
        public string Path { get; private set; }
        public long? BuildingId { get; private set; }
        public long? NeighborhoodId { get; private set; }
        public long? JobListingId { get; private set; }

        private Image(long id, string path, long? buildingId, long? neighborhoodId, long? jobListingId)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Putanja slike ne sme biti prazna.", nameof(path));

            Id = id;
            Path = path;
            BuildingId = buildingId;
            NeighborhoodId = neighborhoodId;
            JobListingId = jobListingId;
        }

        public static Image ForBuilding(long id, string path, long buildingId) =>
            new Image(id, path, buildingId: buildingId, neighborhoodId: null, jobListingId: null);

        public static Image ForNeighborhood(long id, string path, long neighborhoodId) =>
            new Image(id, path, buildingId: null, neighborhoodId: neighborhoodId, jobListingId: null);

        public static Image ForJobListing(long id, string path, long jobListingId) =>
            new Image(id, path, buildingId: null, neighborhoodId: null, jobListingId: jobListingId);

        public Image CopyWithBuildingImageDetails(long? id = null, string? path = null)
        {
            if (BuildingId is null)
                throw new InvalidOperationException(
                    "CopyWithBuildingImageDetails se može pozvati samo na slici koja pripada zgradi.");

            return ForBuilding(id ?? Id, path ?? Path, BuildingId.Value);
        }

        public Image CopyWithNeighborhoodImageDetails(long? id = null, string? path = null)
        {
            if (NeighborhoodId is null)
                throw new InvalidOperationException(
                    "CopyWithNeighborhoodImageDetails se može pozvati samo na slici koja pripada kvartu.");

            return ForNeighborhood(id ?? Id, path ?? Path, NeighborhoodId.Value);
        }

        public Image CopyWithJobListingImageDetails(long? id = null, string? path = null)
        {
            if (JobListingId is null)
                throw new InvalidOperationException(
                    "CopyWithJobListingImageDetails se može pozvati samo na slici koja pripada oglasu usluge.");

            return ForJobListing(id ?? Id, path ?? Path, JobListingId.Value);
        }

        // "Stari" kod:
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