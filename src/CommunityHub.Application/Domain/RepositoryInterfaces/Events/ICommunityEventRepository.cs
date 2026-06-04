using CommunityHub.Application.Domain.Events;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Events
{
    public interface ICommunityEventRepository
    {
        public CommunityEvent Create(CommunityEvent communityEvent);
        public List<CommunityEvent> GetByOrganizerId(long organizerId);
        public List<CommunityEvent> GetByVolunteerCitizenId(long citizenId);
        public List<CommunityEvent> GetByNeighborhoodId(long neighborhoodId);
        public CommunityEvent? GetById(long eventId);
        public void UpdateStatus(long eventId, EventStatus status);
        public int GetOrganizedEventsCount(long organizerId);
    }
}
