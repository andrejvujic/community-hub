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

    public static EventItem CreateFormatted(string name, long eventId = 0)
    {
        var trimmed = name.Trim();
        if (string.IsNullOrEmpty(trimmed))
            throw new ArgumentException("Name must not be empty.", nameof(name));

        var parts = trimmed.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var normalized = string.Join(" ", parts);
        var formatted = char.ToUpper(normalized[0]) + normalized.Substring(1).ToLower();

        return new EventItem(id: 0, name: formatted, eventId: eventId);
    }
}
