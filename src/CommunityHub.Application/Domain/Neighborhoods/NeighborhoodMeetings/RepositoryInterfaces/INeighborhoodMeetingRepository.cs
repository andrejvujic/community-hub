using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings.RepositoryInterfaces
{
    public interface INeighborhoodMeetingRepository
    {
        public List<NeighborhoodMeeting> GetByNeighborhoodId(long neighborhoodId);
        public void UpdateStatus(long meetingId, MeetingStatus newStatus);
        public void UpdateScheduledDate(long meetingId, DateOnly scheduledDate);
        public NeighborhoodMeeting Create(NeighborhoodMeeting meeting);
    }
}
