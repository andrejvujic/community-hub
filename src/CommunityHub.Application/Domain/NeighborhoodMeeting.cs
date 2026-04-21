namespace CommunityHub.Application.Domain
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
    }
}