using CommunityHub.Application.Domain.Events;
using CommunityHub.Application.Services.Dtos.Neighborhoods;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Events
{
    public class CommunityEventDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime EventStart { get; set; }
        public int DurationMinutes { get; set; }
        public int MinVolunteers { get; set; }
        public EventStatus Status { get; set; }
        public NeighborhoodDto Neighborhood { get; set; }
        public CitizenDto Organizer { get; set; }
        public List<EventItemDto> Items { get; set; }
        public List<EventRegistrationDto> Registrations { get; set; }
        public List<EventItemAssignmentDto> Assignments { get; set; }

        public int RegisteredCount => Registrations?.Count ?? 0;

        public CommunityEventDto(
            long id,
            string name,
            string? description,
            DateTime eventStart,
            int durationMinutes,
            int minVolunteers,
            EventStatus status,
            NeighborhoodDto neighborhood,
            CitizenDto organizer,
            List<EventItemDto>? items = null,
            List<EventRegistrationDto>? registrations = null,
            List<EventItemAssignmentDto>? assignments = null
        )
        {
            Id = id;
            Name = name;
            Description = description;
            EventStart = eventStart;
            DurationMinutes = durationMinutes;
            MinVolunteers = minVolunteers;
            Status = status;
            Neighborhood = neighborhood;
            Organizer = organizer;
            Items = items ?? new List<EventItemDto>();
            Registrations = registrations ?? new List<EventRegistrationDto>();
            Assignments = assignments ?? new List<EventItemAssignmentDto>();
        }
    }
}
