using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.MappersDto.Users;

namespace CommunityHub.Application.MappersDto.Events
{
    public class EventRegistrationMapper : IMapper<EventRegistration, EventRegistrationDto>
    {
        private readonly CitizenMapper _citizenMapper = new();

        public EventRegistrationDto MapToDto(EventRegistration eventRegistration)
        {
            if (eventRegistration == null) throw new ArgumentNullException(nameof(eventRegistration));

            return new EventRegistrationDto(
                eventRegistration.Id,
                _citizenMapper.MapToDto(eventRegistration.Citizen),
                eventRegistration.EventId,
                eventRegistration.Attended
            );
        }

        public EventRegistration MapToDomain(EventRegistrationDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new EventRegistration(
                dto.Id,
                _citizenMapper.MapToDomain(dto.Citizen),
                dto.EventId,
                dto.Attended
            );
        }
    }
}
