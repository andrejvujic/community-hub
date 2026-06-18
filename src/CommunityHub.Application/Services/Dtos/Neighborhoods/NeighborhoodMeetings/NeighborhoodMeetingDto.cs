using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings
{
    public class NeighborhoodMeetingDto
    {
        public long Id { get; set; }
        public NeighborhoodDto Neighborhood { get; set; }
        public MeetingTheme Theme { get; set; }
        public TimeOnly MeetingTime { get; set; }
        public DateOnly DateRangeStart { get; set; }
        public DateOnly DateRangeEnd { get; set; }
        public DateOnly? ScheduledDate { get; set; }
        public MeetingStatus Status { get; set; }

        public bool IsTied =>
            Status == MeetingStatus.InPreparation &&
            DateTime.Now >= DateRangeStart.ToDateTime(MeetingTime).AddHours(-24);

        public string MeetingTimeDisplay => $"{MeetingTime:HH:mm} h";

        public string DisplayDate =>
            Status == MeetingStatus.Scheduled && ScheduledDate.HasValue
                ? ScheduledDate.Value.ToString("dd.MM.yyyy.")
                : $"{DateRangeStart:dd.MM.yyyy.} - {DateRangeEnd:dd.MM.yyyy.}";

        public bool IsScheduled => Status == MeetingStatus.Scheduled && ScheduledDate.HasValue;
        public DateTime MeetingTimeAsDateTime => DateTime.Today.Add(MeetingTime.ToTimeSpan());
        public DateTime? ScheduledDateAsDateTime => ScheduledDate?.ToDateTime(TimeOnly.MinValue);
        public DateTime DateRangeStartAsDateTime => DateRangeStart.ToDateTime(TimeOnly.MinValue);
        public DateTime DateRangeEndAsDateTime => DateRangeEnd.ToDateTime(TimeOnly.MinValue);

        public NeighborhoodMeetingDto(
            long id,
            NeighborhoodDto neighborhood,
            MeetingTheme theme,
            TimeOnly meetingTime,
            DateOnly dateRangeStart,
            DateOnly dateRangeEnd,
            MeetingStatus status,
            DateOnly? scheduledDate = null
        )
        {
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
