using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Database.Repositories.Events;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.RepositoryInterfaces.Events;
using CommunityHub.Application.Domain.RepositoryInterfaces.Users;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.MappersDto.Events;
using CommunityHub.Application.Services.Dtos.Events;

namespace CommunityHub.Application.Services.Events
{
    public class CommunityEventService
    {
        private readonly ICommunityEventRepository _eventRepository = new CommunityEventRepository();
        private readonly ICitizenRepository _citizenRepository = new CitizenRepository();
        private readonly IEventItemRepository _eventItemRepository = new EventItemRepository();
        private readonly IMapper<CommunityEvent, CommunityEventDto> _eventMapper = new CommunityEventMapper();
        private readonly EventRegistrationService _registrationService = new();
        private readonly EventItemAssignmentService _assignmentService = new();

        public List<CommunityEventDto> GetByNeighborhoodId(long neighborhoodId)
            => _eventRepository.GetByNeighborhoodId(neighborhoodId)
                .Select(_eventMapper.MapToDto)
                .ToList();

        public List<CommunityEventDto> GetByOrganizerId(long organizerId)
            => _eventRepository.GetByOrganizerId(organizerId)
                .Select(_eventMapper.MapToDto)
                .ToList();

        public List<CommunityEventDto> GetByVolunteerCitizenId(long citizenId)
            => _eventRepository.GetByVolunteerCitizenId(citizenId)
                .Select(_eventMapper.MapToDto)
                .ToList();

        public CommunityEventDto? GetById(long eventId)
            => _eventRepository.GetById(eventId) is CommunityEvent @event
                ? _eventMapper.MapToDto(@event)
                : null;

        public CommunityEventDto CreateEvent(
            long organizerId,
            string name,
            string description,
            DateTime eventStart,
            int durationMinutes,
            int minVolunteers,
            List<string> itemNames)
        {
            var organizer = _citizenRepository.GetById(organizerId);
            var items = itemNames.Distinct().Select(name => EventItem.CreateFormatted(name)).ToList();

            var newEvent = new CommunityEvent(
                id: 0,
                name: name.Trim(),
                description: description.Trim(),
                eventStart: eventStart,
                durationMinutes: durationMinutes,
                minVolunteers: minVolunteers,
                status: EventStatus.Preparation,
                neighborhood: organizer!.Membership!.Neighborhood,
                organizer: organizer,
                items: items
            );

            var createdEvent = _eventRepository.Create(newEvent);
            return _eventMapper.MapToDto(createdEvent);
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

        public void UpdateEventRegistrationAttendance(long registrationId, bool attended)
        {
            _registrationService.UpdateAttendance(registrationId, attended);
        }

        public void UpdateEventStatusIfReady(long eventId)
        {
            var currentEvent = _eventRepository.GetById(eventId);
            if (currentEvent == null)
                return;

            if (currentEvent.CanBeScheduled())
            {
                _eventRepository.UpdateStatus(eventId, EventStatus.Scheduled);
            }
        }
    }
}
