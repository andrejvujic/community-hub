using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Search;

namespace CommunityHub.Application.Modules.Shared.Core.Services.Addresses
{
    public class CityService
    {
        private readonly ICityRepository _cityRepository = Injector.CreateInstance<ICityRepository>();

        public List<City> Search(CitySearchParameters searchParameters) 
            => _cityRepository.Search(searchParameters);    
    }
}
