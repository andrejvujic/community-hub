using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class CountryService
    {
        private readonly ICountryRepository _countryRepository = Injector.CreateInstance<ICountryRepository>();
        private readonly CountryMapper _countruMapper = new();

        public List<Country> Search(CountrySearchParameters searchParameters)
            => _countryRepository.Search(searchParameters);

        public List<Country> GetAll()
            => _countryRepository.GetAll();

        public List<CountryDto> GetAllCountries()
            => _countryRepository.GetAll().Select(_countruMapper.MapToDto).ToList();
    }
}