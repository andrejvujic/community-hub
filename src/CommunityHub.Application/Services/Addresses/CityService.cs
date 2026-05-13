using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Services.Addresses
{
    public class CityService
    {
        private readonly ICityRepository _cityRepository = Injector.CreateInstance<ICityRepository>();

        public List<City> Search(CitySearchParameters searchParameters) 
            => _cityRepository.Search(searchParameters);
        
        public List<City> GetByCountryId(long countryId) 
            => _cityRepository.GetByCountryId(countryId);
    }
}
