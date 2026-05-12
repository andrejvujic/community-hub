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
            => Status == MeetingStatus.InPreparation && DateOnly.FromDateTime(DateTime.Today) >= DateRangeStart.AddDays(-1);

        public string DisplayDate
        {
            get
            {
                if (Status == MeetingStatus.Scheduled && ScheduledDate.HasValue)
                {
                    return ScheduledDate.Value.ToString("dd.MM.yyyy.");
                }
                return $"{DateRangeStart:dd.MM.yyyy.} - {DateRangeEnd:dd.MM.yyyy.}";
            }
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