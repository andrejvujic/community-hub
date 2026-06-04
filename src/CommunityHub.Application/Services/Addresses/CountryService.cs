using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class CountryService : BaseService<Country, CountryDto>
    {
        private readonly ICountryRepository _countryRepository = Injector.CreateInstance<ICountryRepository>();
        private readonly CountryMapper _countryMapper = new();

        public CountryService() : base(mapper: new CountryMapper()) {}

        public CountryDto? GetById(long id)
        { 
            var country = _countryRepository.GetById(id);
            if (country is null) return null;
            return MapToDto(country);
        }

        public List<CountryDto> Search(CountrySearchParameters searchParameters)
            => MapToDto(
                    _countryRepository.Search(searchParameters)
               );

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