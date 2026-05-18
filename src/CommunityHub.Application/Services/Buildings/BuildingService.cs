using CommunityHub.Application.Domain.Buildings;
using CommunityHub.Application.Domain.Buildings.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.Services.Dtos.Buildings;
using CommunityHub.Application.MappersDto.Buildings;

namespace CommunityHub.Application.Services.Buildings
{
    public class BuildingService
    {
        private readonly IBuildingRepository _buildingRepository = Injector.CreateInstance<IBuildingRepository>();
        private readonly BuildingMapper _buildingMapper = new();

        public List<Building> Search(BuildingSearchParameters serachParameters)
            => _buildingRepository.Search(serachParameters);

        public List<BuildingDto> GetByManagerId(long managerId)
        {
            List<Building> buildings = _buildingRepository.GetByManagerId(managerId);
            List<BuildingDto> buildingsDto = [];

            foreach(Building building in buildings)
                buildingsDto.Add(_buildingMapper.MapToDto(building));

            return buildingsDto;
        }

        public Building Create(Building building)
            => _buildingRepository.Create(building);
    }
}