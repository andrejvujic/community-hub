using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Buildings.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Services.Buildings
{
    public class BuildingService
    {
        private readonly IBuildingRepository _buildingRepository = Injector.CreateInstance<IBuildingRepository>();

        public List<Building> Search(BuildingSearchParameters serachParameters)
            => _buildingRepository.Search(serachParameters);
    }
}
