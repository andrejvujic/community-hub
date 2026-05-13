using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Dtos.Events;
using CommunityHub.Application.MappersDto.Neighborhoods;
using CommunityHub.Application.MappersDto.Users;

namespace CommunityHub.Application.MappersDto.Events
{
    public class CommunityEventMapper : IMapper<CommunityEvent, CommunityEventDto>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new();
        private readonly CitizenMapper _citizenMapper = new();
        private readonly EventItemMapper _eventItemMapper = new();
        private readonly EventRegistrationMapper _eventRegistrationMapper = new();
        private readonly EventItemAssignmentMapper _eventItemAssignmentMapper = new();

        public CommunityEventDto MapToDto(CommunityEvent communityEvent)
        {
            if (communityEvent == null) throw new ArgumentNullException(nameof(communityEvent));

            return new CommunityEventDto(
                communityEvent.Id,
                communityEvent.Name,
                communityEvent.Description,
                communityEvent.EventStart,
                communityEvent.DurationMinutes,
                communityEvent.MinVolunteers,
                communityEvent.Status,
                _neighborhoodMapper.MapToDto(communityEvent.Neighborhood),
                _citizenMapper.MapToDto(communityEvent.Organizer),
                communityEvent.Items?.Select(_eventItemMapper.MapToDto).ToList(),
                communityEvent.Registrations?.Select(_eventRegistrationMapper.MapToDto).ToList(),
                communityEvent.Assignments?.Select(_eventItemAssignmentMapper.MapToDto).ToList()
            );
        }

        public CommunityEvent MapToDomain(CommunityEventDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var items = dto.Items?.Select(_eventItemMapper.MapToDomain).ToList();
            var registrations = dto.Registrations?.Select(_eventRegistrationMapper.MapToDomain).ToList();
            var assignments = dto.Assignments?.Select(_eventItemAssignmentMapper.MapToDomain).ToList();

            return new CommunityEvent(
                dto.Id,
                dto.Name,
                dto.Description,
                dto.EventStart,
                dto.DurationMinutes,
                dto.MinVolunteers,
                dto.Status,
                _neighborhoodMapper.MapToDomain(dto.Neighborhood),
                _citizenMapper.MapToDomain(dto.Organizer),
                items,
                registrations,
                assignments
            );
        }
    }
}
