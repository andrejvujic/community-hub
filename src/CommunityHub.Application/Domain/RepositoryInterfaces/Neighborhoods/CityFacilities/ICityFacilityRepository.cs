using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities
{
    public interface ICityFacilityRepository
    {
        CityFacility? GetById(long id);
        List<CityFacility> GetByCityId(long cityId);
    }
}
