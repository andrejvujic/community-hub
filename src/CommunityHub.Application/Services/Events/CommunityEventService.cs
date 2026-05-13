using CommunityHub.Application.Database.Repositories.Events;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.Events.RepositoryInterfaces;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.MappersDto.Events;
using CommunityHub.Application.Services.Dtos.Events;

namespace CommunityHub.Application.Services.Events
{
    public class CommunityEventService
    {
        private readonly ICommunityEventRepository _eventRepository = new CommunityEventRepository();
        private readonly IMapper<CommunityEvent, CommunityEventDto> _eventMapper = new CommunityEventMapper();

        public List<CommunityEventDto> GetByNeighborhoodId(long neighborhoodId)
            => _eventRepository.GetByNeighborhoodId(neighborhoodId)
                .Select(_eventMapper.MapToDto)
                .ToList();

        public CommunityEventDto? GetById(long eventId)
            => _eventRepository.GetById(eventId) is CommunityEvent @event
                ? _eventMapper.MapToDto(@event)
                : null;

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
