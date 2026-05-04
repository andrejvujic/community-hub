using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface ICityRepository
    {
        public List<City> GetByCountryId(long id);
        public List<City> Search(CitySearchParameters searchParameters);
    }
}