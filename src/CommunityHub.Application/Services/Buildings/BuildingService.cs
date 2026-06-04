using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.MappersDto.Buildings;
using CommunityHub.Application.Domain.RepositoryInterfaces.Buildings;
using CommunityHub.Application.Database.Repositories.Buildings;

namespace CommunityHub.Application.Services.Buildings
{
    public class BuildingService : BaseService<Building, BuildingDto>
    {
        private readonly IBuildingRepository _buildingRepository = Injector.CreateInstance<IBuildingRepository>();
        private readonly BuildingMapper _buildingMapper = new();

        public BuildingService() : base(mapper: new BuildingMapper()) { }

        public List<BuildingDto> Search(BuildingSearchParameters serachParameters)
        {
            var buildings = _buildingRepository.Search(serachParameters);
            return MapToDto(buildings);
        }

        public List<BuildingDto> GetByManagerId(long managerId)
        {
            List<Building> buildings = _buildingRepository.GetByManagerId(managerId);
            List<BuildingDto> buildingsDto = [];

            foreach (Building building in buildings)
                buildingsDto.Add(_buildingMapper.MapToDto(building));

            return buildingsDto;
        }

        public Building Create(Building building)
        {
            var newBuilding = _buildingRepository.Create(building);

            return newBuilding;
        }

        public int GetBuildingsCountForManager(long managerId)
        {
            var buildings = _buildingRepository.GetByManagerId(managerId);
            return buildings.Count;
        }
    }
}