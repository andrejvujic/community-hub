using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Services.Addresses
{
    public class CountryService
    {
        private readonly ICountryRepository _countryRepository = Injector.CreateInstance<ICountryRepository>(); 

        public List<Country> Search(CountrySearchParameters searchParameters)
            => _countryRepository.Search(searchParameters);
    }
}
