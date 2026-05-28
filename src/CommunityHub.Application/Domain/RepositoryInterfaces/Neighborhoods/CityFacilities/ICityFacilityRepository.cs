using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities
{
    public interface ICityFacilityRepository
    {
        List<CityFacility> GetByCityId(long cityId);
    }
}
