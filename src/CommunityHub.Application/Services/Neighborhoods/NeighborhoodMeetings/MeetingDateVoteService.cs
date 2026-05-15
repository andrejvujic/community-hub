using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings.RepositoryInterfaces;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings
{
    public class MeetingDateVoteService
    {
        private readonly IMeetingDateVoteRepository _meetingDateVoteRepository
            = Injector.CreateInstance<IMeetingDateVoteRepository>();

        public Dictionary<DateOnly, int> GetVoteCountsForMeeting(long meetingId)
            => _meetingDateVoteRepository.GetVoteCountsForMeeting(meetingId);
    }
}
