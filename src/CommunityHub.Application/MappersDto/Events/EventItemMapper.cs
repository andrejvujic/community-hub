using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Dtos.Events;

namespace CommunityHub.Application.MappersDto.Events
{
    public class EventItemMapper : IMapper<EventItem, EventItemDto>
    {
        public EventItemDto MapToDto(EventItem eventItem)
        {
            if (eventItem == null) throw new ArgumentNullException(nameof(eventItem));

            return new EventItemDto(
                eventItem.Id,
                eventItem.Name,
                eventItem.EventId
            );
        }

        public EventItem MapToDomain(EventItemDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new EventItem(
                dto.Id,
                dto.Name,
                dto.EventId
            );
        }
    }
}
