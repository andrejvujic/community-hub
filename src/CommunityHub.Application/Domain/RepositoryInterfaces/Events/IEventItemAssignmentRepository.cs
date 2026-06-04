using CommunityHub.Application.Domain.Events;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Events
{
    public interface IEventItemAssignmentRepository
    {
        public List<EventItemAssignment> GetByEventId(long eventId);
        public EventItemAssignment Create(EventItemAssignment assignment);
    }
}
