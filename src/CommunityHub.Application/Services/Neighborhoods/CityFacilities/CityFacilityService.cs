using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities;
using CommunityHub.Application.MappersDto.Neighborhoods.CityFacilities;
using CommunityHub.Application.Services.Dtos.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Services.Neighborhoods.CityFacilities
{
    public class CityFacilityService : BaseService<CityFacility, CityFacilityDto>
    {
        private readonly ICityFacilityRepository _facilityRepository = Injector.CreateInstance<ICityFacilityRepository>();
        private readonly CityFacilityMapper _facilityMapper = new();

        public CityFacilityService() : base(mapper: new CityFacilityMapper()) { }

        public List<CityFacilityDto> GetByCityId(long cityId)
        {
            var facilities = _facilityRepository.GetByCityId(cityId);
            return MapToDto(facilities);
        }
    }
}
