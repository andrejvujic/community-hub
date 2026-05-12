namespace CommunityHub.Application.Services.Dtos.Images
{
    public class ImageDto
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public long? BuildingId { get; set; }
        public long? NeighborhoodId { get; set; }
        public long? JobListingId { get; set; }

        public ImageDto(
            long id,
            string path,
            long? buildingId = null,
            long? neighborhoodId = null,
            long? jobListingId = null
        )
        {
            Id = id;
            Path = path;
            BuildingId = buildingId;
            NeighborhoodId = neighborhoodId;
            JobListingId = jobListingId;
        }
    }
}