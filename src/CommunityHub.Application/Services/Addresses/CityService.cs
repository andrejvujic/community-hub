using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
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
        
        public List<City> GetByCountryId(long countryId) 
            => _cityRepository.GetByCountryId(countryId);

        public List<CityDto> GetByCountry(long countryId) 
            => _cityRepository.GetByCountryId(countryId).Select(_cityMapper.MapToDto).ToList();
    }
}
