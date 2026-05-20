using CommunityHub.Application.Domain.Addresses;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Addresses
{
    public interface IStreetRepository
    {
        public List<Street> GetByCityId(long id);
    }
}