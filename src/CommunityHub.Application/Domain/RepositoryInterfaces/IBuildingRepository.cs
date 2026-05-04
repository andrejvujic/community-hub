using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IBuildingRepository
    {
        public Building Create(Building building);
        public List<Building> Search(BuildingSearchParameters searchParameters);
        public List<Building> GetByManagerId(long managerId);
    }
}