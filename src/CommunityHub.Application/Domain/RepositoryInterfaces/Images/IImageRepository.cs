using CommunityHub.Application.Database.Mapping.EntityMappers;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Images
{
    public interface IImageRepository
    {
        public Image Create(Image image);

        public List<Image> GetByBuilding(long buildingId);
        public List<Image> GetByNeighborhoodId(long neighborhoodId);
        public List<Image> GetByJobListingId(long jobListingId);

        public void DeleteByBuildingId(long buildingId);
        public void DeleteByNeighborhoodId(long neighborhoodId);
        public void DeleteByJobListingId(long jobListingId);
    }
}