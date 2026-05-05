using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface ICountryRepository
    {
        public List<Country> GetAll();
        public List<Country> Search(CountrySearchParameters searchParameters);
    }
}