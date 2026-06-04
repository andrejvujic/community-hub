using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;
using CommunityHub.Application.Helpers.Search;
using CommunityHub.Application.MappersDto.Addresses;
using CommunityHub.Application.Services.Dtos.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class CityService : BaseService<City, CityDto>
    {
        private readonly ICityRepository _cityRepository = Injector.CreateInstance<ICityRepository>();
        private readonly CityMapper _cityMapper = new();

        public CityService() : base(mapper: new CityMapper()) {}

        public CityDto? GetById(long id)
        {
            var city  = _cityRepository.GetById(id);
            if (city is null) return null;
            return MapToDto(city);
        }

        public List<CityDto> Search(CitySearchParameters searchParameters)
            => MapToDto(
                    _cityRepository.Search(searchParameters)
               );
        
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