using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodMeetings
{
    public interface INeighborhoodMeetingRepository
    {
        public NeighborhoodMeeting? GetById(long meetingId);
        public List<NeighborhoodMeeting> GetByNeighborhoodId(long neighborhoodId);
        public void UpdateStatus(long meetingId, MeetingStatus newStatus);
        public void UpdateScheduledDate(long meetingId, DateOnly scheduledDate);
        public NeighborhoodMeeting Create(NeighborhoodMeeting meeting);
        public List<NeighborhoodMeeting> GetByCoordinatorId(long coordinatorId);
    }
}
