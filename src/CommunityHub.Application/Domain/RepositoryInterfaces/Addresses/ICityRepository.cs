using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Addresses
{
    public interface ICityRepository
    {
        public City? GetById(long id);
        public List<City> GetByCountryId(long id);
        public List<City> Search(CitySearchParameters searchParameters);
    }
}