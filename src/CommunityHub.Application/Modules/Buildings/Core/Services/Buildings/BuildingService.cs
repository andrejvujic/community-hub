using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Buildings
{
    public class BuildingService
    {
        private readonly IBuildingRepository _buildingRepository = Injector.CreateInstance<IBuildingRepository>();

        public List<Building> Search(BuildingSearchParameters serachParameters)
            => _buildingRepository.Search(serachParameters);
    }
}
