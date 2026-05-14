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
        private readonly CountryMapper _countryMapper = new();

        public List<Country> Search(CountrySearchParameters searchParameters)
            => _countryRepository.Search(searchParameters);

        public List<CountryDto> GetAll()
        {
            List<Country> countries = _countryRepository.GetAll();
            List<CountryDto> countriesDto = [];

            foreach (Country country in countries)
                countriesDto.Add(_countryMapper.MapToDto(country));

            return countriesDto;
        }
    }
}