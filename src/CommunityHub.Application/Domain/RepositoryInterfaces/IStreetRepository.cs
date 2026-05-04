
namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IStreetRepository
    {
        public List<Street> GetByCityId(long id);
    }
}