using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Events
{
    public class EventRegistrationDto
    {
        public long Id { get; set; }
        public CitizenDto Citizen { get; set; }
        public long EventId { get; set; }
        public bool? Attended { get; set; }

        public EventRegistrationDto(long id, CitizenDto citizen, long eventId, bool? attended = null)
        {
            Id = id;
            Citizen = citizen;
            EventId = eventId;
            Attended = attended;
        }
    }
}
