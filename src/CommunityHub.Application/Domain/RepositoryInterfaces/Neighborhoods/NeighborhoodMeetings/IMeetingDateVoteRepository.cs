using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodMeetings
{
    public interface IMeetingDateVoteRepository
    {
        public MeetingDateVote Create(MeetingDateVote vote);
        public void UpdateDate(long voteId, DateOnly newDate);
        public MeetingDateVote? GetByMeetingIdAndCitizenId(long meetingId, long citizenId);
        public Dictionary<DateOnly, int> GetVoteCountsForMeeting(long meetingId);
    }
}
