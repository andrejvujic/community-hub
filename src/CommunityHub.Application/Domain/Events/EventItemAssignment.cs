namespace CommunityHub.Application.Domain.Events;

public class EventItemAssignment
{
    public long Id { get; private set; }
    public EventRegistration Registration { get; private set; }
    public EventItem Item { get; private set; }

    public EventItemAssignment(long id, EventRegistration registration, EventItem item)
    {
        if (registration == null) throw new ArgumentNullException(nameof(registration));
        if (item == null) throw new ArgumentNullException(nameof(item));

        Id = id;
        Registration = registration;
        Item = item;
    }

    public EventItemAssignment CopyWith(long? id = null, EventRegistration? registration = null, EventItem? item = null)
    {
        return new EventItemAssignment(
            id ?? Id,
            registration ?? Registration,
            item ?? Item
        );
    }
}
