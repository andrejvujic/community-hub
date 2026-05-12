using CommunityHub.Application.Domain.Events;

namespace CommunityHub.Application.Domain.Events.RepositoryInterfaces
{
    public interface IEventItemRepository
    {
        public EventItem Create(EventItem item);
        public List<EventItem> GetByEventId(long eventId);
    }
}
