using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.Events.RepositoryInterfaces;

namespace CommunityHub.Application.Services.Events
{
    public class CommunityEventService
    {
        private readonly ICommunityEventRepository _eventRepository;

        public CommunityEventService(ICommunityEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public void UpdateEventStatusesForNeighborhood(long neighborhoodId)
        {
            var events = _eventRepository.GetByNeighborhoodId(neighborhoodId);

            UpdatePreparationEvents(events);
            UpdateScheduledEvents(events);
        }

        private void UpdatePreparationEvents(List<CommunityEvent> events)
        {
            var preparationEvents = events.Where(e => e.Status == EventStatus.Preparation).ToList();

            foreach (var ev in preparationEvents)
            {
                if (ev.ShouldBeCancelled())
                {
                    _eventRepository.UpdateStatus(ev.Id, EventStatus.Cancelled);
                }
            }
        }

        private void UpdateScheduledEvents(List<CommunityEvent> events)
        {
            var scheduledEvents = events.Where(e => e.Status == EventStatus.Scheduled).ToList();

            foreach (var ev in scheduledEvents)
            {
                if (ev.HasFinished())
                {
                    _eventRepository.UpdateStatus(ev.Id, EventStatus.Finished);
                }
            }
        }
    }
}
