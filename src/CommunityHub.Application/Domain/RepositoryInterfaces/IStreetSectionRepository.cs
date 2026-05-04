
namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface IStreetSectionRepository
    {
        public int GetStartNumber(long? streetId, long? cityId);
        public List<StreetSection> GetByNeighborhoodId(long neighborhoodId);
        public int GetEndNumber(long? streetId, long? cityId);
        public StreetSection Create(StreetSection section, long neighborhoodId);
    }
}