using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodMeetings
{
    public class NeighborhoodMeetingMapper : IMapper<NeighborhoodMeeting, NeighborhoodMeetingDto>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new();

        public NeighborhoodMeetingDto MapToDto(NeighborhoodMeeting neighborhoodMeeting)
        {
            if(neighborhoodMeeting == null) throw new ArgumentNullException(nameof(neighborhoodMeeting));

            return new NeighborhoodMeetingDto(
                neighborhoodMeeting.Id,
                _neighborhoodMapper.MapToDto(neighborhoodMeeting.Neighborhood),
                neighborhoodMeeting.Theme,
                neighborhoodMeeting.MeetingTime,
                neighborhoodMeeting.DateRangeStart,
                neighborhoodMeeting.DateRangeEnd,
                neighborhoodMeeting.Status,
                neighborhoodMeeting.ScheduledDate
            );
        }

        public NeighborhoodMeeting MapToEntity(NeighborhoodMeetingDto dto)
        {
            if(dto == null) throw new ArgumentNullException(nameof(dto));

            return new NeighborhoodMeeting(
                _neighborhoodMapper.MapToEntity(dto.Neighborhood),
                dto.Theme,
                dto.MeetingTime,
                dto.DateRangeStart,
                dto.DateRangeEnd,
                dto.Status,
                dto.ScheduledDate,
                dto.Id
            );
        }
    }
}
