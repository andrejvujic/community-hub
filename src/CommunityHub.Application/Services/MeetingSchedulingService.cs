using CommunityHub.Application.Database.Repositories.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.Services
{
    public class MeetingSchedulingService
    {
        private readonly NeighborhoodMeetingRepository _meetingRepository;
        private readonly MeetingDateVoteRepository _voteRepository;

        public MeetingSchedulingService(NeighborhoodMeetingRepository meetingRepository, MeetingDateVoteRepository voteRepository)
        {
            _meetingRepository = meetingRepository;
            _voteRepository = voteRepository;
        }

        public void ProcessMeetingsInPreparation(List<NeighborhoodMeeting> meetings)
        {
            var now = DateOnly.FromDateTime(DateTime.Today);

            foreach (var meeting in meetings.Where(m => m.Status == MeetingStatus.InPreparation))
            {
                if (now >= meeting.DateRangeStart.AddDays(-1))
                {
                    ResolveMeetingStatus(meeting);
                }
            }
        }

        private void ResolveMeetingStatus(NeighborhoodMeeting meeting)
        {
            var voteCounts = _voteRepository.GetVoteCountsForMeeting(meeting.Id);

            if (voteCounts.Count == 0)
            {
                _meetingRepository.UpdateStatus(meeting.Id, MeetingStatus.Cancelled);
                return;
            }

            int maxVotes = voteCounts.Values.Max();
            var topDates = voteCounts.Where(v => v.Value == maxVotes).Select(v => v.Key).ToList();

            if (topDates.Count == 1)
            {
                _meetingRepository.UpdateScheduledDate(meeting.Id, topDates.First());
                _meetingRepository.UpdateStatus(meeting.Id, MeetingStatus.Scheduled);
            }
            else if (DateOnly.FromDateTime(DateTime.Today) >= meeting.DateRangeStart)
                _meetingRepository.UpdateStatus(meeting.Id, MeetingStatus.Cancelled);
        }

        public List<DateOnly> GetTiedDates(long meetingId)
        {
            var voteCounts = _voteRepository.GetVoteCountsForMeeting(meetingId);
            if (voteCounts.Count == 0) return new List<DateOnly>();

            int maxVotes = voteCounts.Values.Max();
            return voteCounts
                .Where(v => v.Value == maxVotes)
                .Select(v => v.Key)
                .OrderBy(d => d)
                .ToList();
        }

        public bool HasTiedDates(long meetingId)
        {
            var voteCounts = _voteRepository.GetVoteCountsForMeeting(meetingId);
            if (voteCounts.Count == 0) return false;

            int maxVotes = voteCounts.Values.Max();
            return voteCounts.Count(v => v.Value == maxVotes) > 1;
        }
    }
}
