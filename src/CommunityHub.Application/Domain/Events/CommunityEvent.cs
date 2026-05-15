using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.Domain.Users;
using System.Collections.Generic;
using System.Linq;

namespace CommunityHub.Application.Domain.Events;

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

    public List<EventItem> Items { get; private set; } = new List<EventItem>();
    public List<EventRegistration> Registrations { get; private set; } = new List<EventRegistration>();
    public List<EventItemAssignment> Assignments { get; private set; } = new List<EventItemAssignment>();

    public int RegisteredCount => Registrations.Count;

    public bool AllItemsAssigned => Items.All(i => Assignments.Any(a => a.Item.Id == i.Id));

    public CommunityEvent(
        long id,
        string name,
        string? description,
        DateTime eventStart,
        int durationMinutes,
        int minVolunteers,
        EventStatus status,
        Neighborhood neighborhood,
        Citizen organizer,
        List<EventItem>? items = null,
        List<EventRegistration>? registrations = null,
        List<EventItemAssignment>? assignments = null)
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

        Items = items ?? new List<EventItem>();
        Registrations = registrations ?? new List<EventRegistration>();
        Assignments = assignments ?? new List<EventItemAssignment>();
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
        Citizen? organizer = null,
        List<EventItem>? items = null,
        List<EventRegistration>? registrations = null,
        List<EventItemAssignment>? assignments = null)
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
            organizer ?? Organizer,
            items ?? Items,
            registrations ?? Registrations,
            assignments ?? Assignments
        );
    }

    public bool ShouldBeCancelled()
    {
        var timeToEvent = EventStart - DateTime.Now;
        bool enoughVolunteers = Registrations.Count >= MinVolunteers;
        bool allItemsAssigned = AllItemsAssigned;

        return timeToEvent.TotalHours < 12 && (!enoughVolunteers || !allItemsAssigned);
    }

    public bool HasFinished()
    {
        return DateTime.Now > EventStart.AddMinutes(DurationMinutes);
    }

    public static bool TryParseEventStart(
        DateTime? selectedDate,
        string hour,
        string minute,
        out DateTime eventStart)
    {
        eventStart = default;

        if (!selectedDate.HasValue)
            return false;

        if (!int.TryParse(hour, out var h) || h < 0 || h > 23)
            return false;

        if (!int.TryParse(minute, out var m) || m < 0 || m > 59)
            return false;

        var date = selectedDate.Value.Date;
        eventStart = DateTime.SpecifyKind(date + new TimeSpan(h, m, 0), DateTimeKind.Unspecified);

        return eventStart > DateTime.Now.AddHours(12);
    }

    public static bool TryParseDuration(
        string durationHour,
        string durationMinute,
        out int durationMinutes)
    {
        durationMinutes = 0;

        if (!int.TryParse(durationHour, out var dh) || dh < 0)
            return false;

        if (!int.TryParse(durationMinute, out var dm) || dm < 0 || dm > 59)
            return false;

        durationMinutes = dh * 60 + dm;
        return durationMinutes > 0;
    }

    public static bool TryParseMinVolunteers(
        string volunteers,
        out int minVolunteers)
    {
        minVolunteers = 0;

        if (!int.TryParse(volunteers, out var v))
            return false;

        if (v <= 0)
            return false;

        minVolunteers = v;
        return true;
    }
}
