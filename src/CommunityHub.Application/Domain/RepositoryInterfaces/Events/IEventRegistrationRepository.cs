using CommunityHub.Application.Domain.Events;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Events
{
    public interface IEventRegistrationRepository
    {
        public List<EventRegistration> GetByEventId(long eventId);
        public EventRegistration Create(EventRegistration registration);
        public void UpdateAttendance(long registrationId, bool? attended);
        public int GetConfirmedVolunteerCount(long citizenId);
    }
}
