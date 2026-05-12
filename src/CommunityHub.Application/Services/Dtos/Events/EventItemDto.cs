namespace CommunityHub.Application.Services.Dtos.Events
{
    public class EventItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long EventId { get; set; }

        public EventItemDto(long id, string name, long eventId)
        {
            Id = id;
            Name = name;
            EventId = eventId;
        }
    }
}
