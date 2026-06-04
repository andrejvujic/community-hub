using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Dtos.Events;

namespace CommunityHub.Application.MappersDto.Events
{
    public class EventItemAssignmentMapper : IMapper<EventItemAssignment, EventItemAssignmentDto>
    {
        private readonly EventRegistrationMapper _eventRegistrationMapper = new();
        private readonly EventItemMapper _eventItemMapper = new();

        public EventItemAssignmentDto MapToDto(EventItemAssignment eventItemAssignment)
        {
            if (eventItemAssignment == null) throw new ArgumentNullException(nameof(eventItemAssignment));

            return new EventItemAssignmentDto(
                eventItemAssignment.Id,
                _eventRegistrationMapper.MapToDto(eventItemAssignment.Registration),
                _eventItemMapper.MapToDto(eventItemAssignment.Item)
            );
        }

        public EventItemAssignment MapToEntity(EventItemAssignmentDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new EventItemAssignment(
                dto.Id,
                _eventRegistrationMapper.MapToEntity(dto.Registration),
                _eventItemMapper.MapToEntity(dto.Item)
            );
        }
    }
}
