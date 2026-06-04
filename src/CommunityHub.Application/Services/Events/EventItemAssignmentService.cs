using CommunityHub.Application.Database.Repositories.Events;
using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Domain.RepositoryInterfaces.Events;
using CommunityHub.Application.MappersDto;
using CommunityHub.Application.MappersDto.Events;
using CommunityHub.Application.Services.Dtos.Events;

namespace CommunityHub.Application.Services.Events
{
    public class EventItemAssignmentService
    {
        private readonly IEventItemAssignmentRepository _assignmentRepository = new EventItemAssignmentRepository();
        private readonly IEventRegistrationRepository _registrationRepository = new EventRegistrationRepository();
        private readonly IEventItemRepository _itemRepository = new EventItemRepository();
        private readonly IMapper<EventItemAssignment, EventItemAssignmentDto> _assignmentMapper = new EventItemAssignmentMapper();

        public List<EventItemAssignmentDto> GetByEventId(long eventId)
            => _assignmentRepository.GetByEventId(eventId)
                .Select(_assignmentMapper.MapToDto)
                .ToList();

        public EventItemAssignmentDto? CreateAssignment(long registrationId, long itemId, long eventId)
        {
            var registrations = _registrationRepository.GetByEventId(eventId);
            var registration = registrations.FirstOrDefault(r => r.Id == registrationId);

            var items = _itemRepository.GetByEventId(eventId);
            var item = items.FirstOrDefault(i => i.Id == itemId);

            if (registration == null || item == null)
                return null;

            var assignment = new EventItemAssignment(0, registration, item);
            var created = _assignmentRepository.Create(assignment);
            return _assignmentMapper.MapToDto(created);
        }
    }
}
