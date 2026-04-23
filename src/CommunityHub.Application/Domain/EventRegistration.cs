using System;

namespace CommunityHub.Application.Domain;

public class EventRegistration
{
    public long Id { get; private set; }
    public Citizen Citizen { get; private set; }
    public long EventId { get; private set; }
    public bool? Attended { get; private set; }

    public EventRegistration(long id, Citizen citizen, long eventId, bool? attended = null)
    {
        if (citizen == null) throw new ArgumentNullException(nameof(citizen));

        Id = id;
        Citizen = citizen;
        EventId = eventId;
        Attended = attended;
    }

    public EventRegistration CopyWith(
        long? id = null,
        Citizen? citizen = null,
        long? eventId = null,
        bool? attended = null)
    {
        return new EventRegistration(
            id ?? Id,
            citizen ?? Citizen,
            eventId ?? EventId,
            attended ?? Attended
        );
    }

    public EventRegistration MarkAttendance(bool attended)
    {
        return CopyWith(attended: attended);
    }
}
