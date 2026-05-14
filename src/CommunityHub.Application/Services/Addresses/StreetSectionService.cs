using CommunityHub.Application.Domain.Addresses.RepositoryInterfaces;

namespace CommunityHub.Application.Services.Addresses
{
    public class StreetSectionService
    {
        private readonly IStreetSectionRepository _streetSectionRepository = Injector.CreateInstance<IStreetSectionRepository>();

        public int GetStartNumber(long? streetId, long? cityId)
            => _streetSectionRepository.GetStartNumber(streetId, cityId);

        public int GetEndNumber(long? streetId, long? cityId)
            => _streetSectionRepository.GetEndNumber(streetId, cityId);
    }
}