using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class CityService
    {
        private readonly ICityRepository _cityRepository = Injector.CreateInstance<ICityRepository>();
        private readonly CityMapper _cityMapper = new();
        public List<City> Search(CitySearchParameters searchParameters) 
            => _cityRepository.Search(searchParameters);
        
        public List<CityDto> GetByCountryId(long countryId)
        {
            List<City> cities = _cityRepository.GetByCountryId(countryId);
            List<CityDto> citiesDto = [];

            foreach (City city in cities)
                citiesDto.Add(_cityMapper.MapToDto(city));

            return citiesDto;
        }
         
    }
}