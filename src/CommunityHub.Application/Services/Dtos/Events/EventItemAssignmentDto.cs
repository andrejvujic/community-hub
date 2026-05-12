namespace CommunityHub.Application.Services.Dtos.Events
{
    public class EventItemAssignmentDto
    {
        public long Id { get; set; }
        public EventRegistrationDto Registration { get; set; }
        public EventItemDto Item { get; set; }

        public EventItemAssignmentDto(long id, EventRegistrationDto registration, EventItemDto item)
        {
            Id = id;
            Registration = registration;
            Item = item;
        }
    }
}
