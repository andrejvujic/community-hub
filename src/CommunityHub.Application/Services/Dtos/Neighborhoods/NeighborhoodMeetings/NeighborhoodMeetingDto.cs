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
