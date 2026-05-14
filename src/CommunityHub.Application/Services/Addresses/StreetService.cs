using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;
using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.Services.Addresses
{
    public class StreetService
    {
        private readonly IStreetRepository _streetRepository = Injector.CreateInstance<IStreetRepository>();

        public List<Street> GetByCityId(long cityId)
            => _streetRepository.GetByCityId(cityId);
    }
}