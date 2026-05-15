using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings
{
    public class NeighborhoodMeetingService
    {
        private readonly INeighborhoodMeetingRepository _neighborhoodMeetingRepository 
            = Injector.CreateInstance<INeighborhoodMeetingRepository>();
        private readonly NeighborhoodMeetingMapper _neighborhoodMeetingMapper = new();

        public List<NeighborhoodMeetingDto> GetByNeighborhood(long neighborhoodId)
            => _neighborhoodMeetingRepository.GetByNeighborhoodId(neighborhoodId)
                .Select(_neighborhoodMeetingMapper.MapToDto)
                .
            ToList();
        public NeighborhoodMeetingDto Create(NeighborhoodMeetingDto neighborhoodMeetingDto)
        {
            var neighborhoodMeeting = _neighborhoodMeetingMapper.MapToDomain(neighborhoodMeetingDto);
            var created = _neighborhoodMeetingRepository.Create(neighborhoodMeeting);
            return _neighborhoodMeetingMapper.MapToDto(created);
        }

        public void UpdateStatus(long meetingId, MeetingStatus newStatus)
            => _neighborhoodMeetingRepository.UpdateStatus(meetingId, newStatus);

        public void UpdateScheduledDate(long meetingId, DateOnly scheduledDate)
            => _neighborhoodMeetingRepository.UpdateScheduledDate(meetingId, scheduledDate);
    }
}
