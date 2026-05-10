using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Modules.Shared.Core.Services.Addresses
{
    public class CountryService
    {
        private readonly ICountryRepository _countryRepository = Injector.CreateInstance<ICountryRepository>(); 

        public List<Country> Search(CountrySearchParameters searchParameters)
            => _countryRepository.Search(searchParameters);
    }
}
