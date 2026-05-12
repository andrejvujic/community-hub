namespace CommunityHub.Application.Domain.Events;

public class EventItem
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public long EventId { get; private set; }

    public EventItem(long id, string name, long eventId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name must not be empty.", nameof(name));

        Id = id;
        Name = name;
        EventId = eventId;
    }

    public EventItem CopyWith(long? id = null, string? name = null, long? eventId = null)
    {
        return new EventItem(
            id ?? Id,
            name ?? Name,
            eventId ?? EventId
        );
    }
}
