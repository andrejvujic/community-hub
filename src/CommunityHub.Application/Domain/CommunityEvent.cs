namespace CommunityHub.Application.Domain;

public class CommunityEvent
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime EventStart { get; private set; }
    public int DurationMinutes { get; private set; }
    public int MinVolunteers { get; private set; }
    public EventStatus Status { get; private set; }
    public Neighborhood Neighborhood { get; private set; }
    public Citizen Organizer { get; private set; }

    public CommunityEvent(
        long id,
        string name,
        string? description,
        DateTime eventStart,
        int durationMinutes,
        int minVolunteers,
        EventStatus status,
        Neighborhood neighborhood,
        Citizen organizer)
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
    }

    public CommunityEvent CopyWith(
        long? id = null,
        string? name = null,
        string? description = null,
        DateTime? eventStart = null,
        int? durationMinutes = null,
        int? minVolunteers = null,
        EventStatus? status = null,
        Neighborhood? neighborhood = null,
        Citizen? organizer = null)
    {
        return new CommunityEvent(
            id ?? Id,
            name ?? Name,
            description ?? Description,
            eventStart ?? EventStart,
            durationMinutes ?? DurationMinutes,
            minVolunteers ?? MinVolunteers,
            status ?? Status,
            neighborhood ?? Neighborhood,
            organizer ?? Organizer
        );
    }
}
