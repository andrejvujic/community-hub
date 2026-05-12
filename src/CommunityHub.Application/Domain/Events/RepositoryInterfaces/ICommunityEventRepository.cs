using CommunityHub.Application.Domain.Events;

namespace CommunityHub.Application.Domain.Events.RepositoryInterfaces
{
    public interface ICommunityEventRepository
    {
        public CommunityEvent Create(CommunityEvent communityEvent);
        public List<CommunityEvent> GetByOrganizerId(long organizerId);
        public List<CommunityEvent> GetByVolunteerCitizenId(long citizenId);
        public List<CommunityEvent> GetByNeighborhoodId(long neighborhoodId);
    }
}
