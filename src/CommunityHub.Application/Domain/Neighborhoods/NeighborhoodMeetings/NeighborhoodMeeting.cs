using CommunityHub.Application.Domain.Neighborhoods;

namespace CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings
{
    public class NeighborhoodMeeting
    {
        public long Id { get; private set; }
        public Neighborhood Neighborhood { get; private set; }
        public MeetingTheme Theme { get; private set; }
        public TimeOnly MeetingTime { get; private set; }
        public DateOnly DateRangeStart { get; private set; }
        public DateOnly DateRangeEnd { get; private set; }
        public DateOnly? ScheduledDate { get; private set; }
        public MeetingStatus Status { get; private set; }

        public bool IsTied
            => Status == MeetingStatus.InPreparation &&
               DateTime.Now >= DateRangeStart.ToDateTime(MeetingTime).AddHours(-24);

        public bool IsExpired
            => Status == MeetingStatus.InPreparation &&
               DateTime.Now >= DateRangeStart.ToDateTime(MeetingTime).AddHours(-3);

        public MeetingOutcome DetermineOutcome(Dictionary<DateOnly, int> voteCounts)
        {
            if (!IsTied && !IsExpired)
                return MeetingOutcome.TooEarly;

            if (voteCounts.Count == 0 || IsExpired)
                return MeetingOutcome.Cancelled;

            int maxVotes = voteCounts.Values.Max();
            bool hasTie = voteCounts.Count(v => v.Value == maxVotes) > 1;

            return hasTie ? MeetingOutcome.Tied : MeetingOutcome.Resolved;
        }

        public static DateOnly? ResolveScheduledDate(Dictionary<DateOnly, int> voteCounts)
        {
            if (voteCounts.Count == 0) return null;

            int maxVotes = voteCounts.Values.Max();
            var topDates = voteCounts.Where(v => v.Value == maxVotes).Select(v => v.Key).ToList();

            return topDates.Count == 1 ? topDates[0] : null;
        }

        public static bool HasTiedDates(Dictionary<DateOnly, int> voteCounts)
        {
            if (voteCounts.Count == 0) return false;
            int maxVotes = voteCounts.Values.Max();
            return voteCounts.Count(v => v.Value == maxVotes) > 1;
        }

        public static List<DateOnly> GetTiedDates(Dictionary<DateOnly, int> voteCounts)
        {
            if (voteCounts.Count == 0) return new List<DateOnly>();
            int maxVotes = voteCounts.Values.Max();
            return voteCounts
                .Where(v => v.Value == maxVotes)
                .Select(v => v.Key)
                .OrderBy(d => d)
                .ToList();
        }

        public NeighborhoodMeeting(
            Neighborhood neighborhood,
            MeetingTheme theme,
            TimeOnly meetingTime,
            DateOnly dateRangeStart,
            DateOnly dateRangeEnd,
            MeetingStatus status = MeetingStatus.InPreparation,
            DateOnly? scheduledDate = null,
            long id = 0
        )
        {
            if (dateRangeStart > dateRangeEnd) throw new ArgumentException("Invalid date range.");

            Id = id;
            Neighborhood = neighborhood;
            Theme = theme;
            MeetingTime = meetingTime;
            DateRangeStart = dateRangeStart;
            DateRangeEnd = dateRangeEnd;
            Status = status;
            ScheduledDate = scheduledDate;
        }

        public NeighborhoodMeeting CopyWith(
            long? id = null,
            Neighborhood? neighborhood = null,
            MeetingTheme? theme = null,
            TimeOnly? meetingTime = null,
            DateOnly? dateRangeStart = null,
            DateOnly? dateRangeEnd = null,
            DateOnly? scheduledDate = null,
            MeetingStatus? status = null
        )
        {
            return new NeighborhoodMeeting(
                neighborhood ?? Neighborhood,
                theme ?? Theme,
                meetingTime ?? MeetingTime,
                dateRangeStart ?? DateRangeStart,
                dateRangeEnd ?? DateRangeEnd,
                status ?? Status,
                scheduledDate ?? ScheduledDate,
                id ?? Id
            );
        }
    }
}