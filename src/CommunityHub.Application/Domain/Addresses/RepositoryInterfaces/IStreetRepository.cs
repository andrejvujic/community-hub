namespace CommunityHub.Application.Domain.Addresses.RepositoryInterfaces
{
    public interface IStreetRepository
    {
        public List<Street> GetByCityId(long id);
    }
}