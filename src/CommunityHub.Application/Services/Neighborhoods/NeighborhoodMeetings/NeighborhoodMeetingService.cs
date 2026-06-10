using CommunityHub.Application.Database.Repositories.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings
{
    public class NeighborhoodMeetingService
    {
        private readonly INeighborhoodMeetingRepository _neighborhoodMeetingRepository 
            = Injector.CreateInstance<INeighborhoodMeetingRepository>();
        private readonly IMeetingDateVoteRepository _meetingDateVoteRepository
            = Injector.CreateInstance<IMeetingDateVoteRepository>();
        private readonly NeighborhoodMeetingMapper _neighborhoodMeetingMapper = new();

        /*
        public List<NeighborhoodMeetingDto> GetByNeighborhood(long neighborhoodId)
            => _neighborhoodMeetingRepository.GetByNeighborhoodId(neighborhoodId)
                .Select(_neighborhoodMeetingMapper.MapToDto)
                .ToList();
        */
        public List<NeighborhoodMeetingDto> GetByNeighborhood(long neighborhoodId)
        {
            var meetings = _neighborhoodMeetingRepository.GetByNeighborhoodId(neighborhoodId);

            var voteCounts = meetings.ToDictionary(
                m => m.Id,
                m => _meetingDateVoteRepository.GetVoteCountsForMeeting(m.Id));

            foreach (var meeting in meetings.Where(m => m.IsTied || m.IsExpired))
                ProcessSingleMeeting(meeting, voteCounts[meeting.Id]);

            return _neighborhoodMeetingRepository.GetByNeighborhoodId(neighborhoodId)
                .Select(_neighborhoodMeetingMapper.MapToDto)
                .ToList();
        }

        public List<DateOnly> GetTiedDatesForMeeting(long meetingId)
        {
            var voteCounts = _meetingDateVoteRepository.GetVoteCountsForMeeting(meetingId);
            return NeighborhoodMeeting.GetTiedDates(voteCounts);
        }

        public NeighborhoodMeetingDto Create(NeighborhoodMeetingDto neighborhoodMeetingDto)
        {
            var neighborhoodMeeting = _neighborhoodMeetingMapper.MapToEntity(neighborhoodMeetingDto);
            var created = _neighborhoodMeetingRepository.Create(neighborhoodMeeting);
            return _neighborhoodMeetingMapper.MapToDto(created);
        }

        public void UpdateStatus(long meetingId, MeetingStatus newStatus)
            => _neighborhoodMeetingRepository.UpdateStatus(meetingId, newStatus);

        public void UpdateScheduledDate(long meetingId, DateOnly scheduledDate)
            => _neighborhoodMeetingRepository.UpdateScheduledDate(meetingId, scheduledDate);

        public void ProcessPendingMeetings(long neighborhoodId, Dictionary<long, Dictionary<DateOnly, int>> voteCounts)
        {
            var meetings = _neighborhoodMeetingRepository.GetByNeighborhoodId(neighborhoodId);

            foreach (var meeting in meetings.Where(m => m.IsTied || m.IsExpired))
                ProcessSingleMeeting(meeting, voteCounts[meeting.Id]);
        }

        private void ProcessSingleMeeting(NeighborhoodMeeting meeting, Dictionary<DateOnly, int> voteCounts)
        {
            switch (meeting.DetermineOutcome(voteCounts))
            {
                case MeetingOutcome.Cancelled:
                    _neighborhoodMeetingRepository.UpdateStatus(meeting.Id, MeetingStatus.Cancelled);
                    break;

                case MeetingOutcome.Resolved:
                    var date = NeighborhoodMeeting.ResolveScheduledDate(voteCounts)!.Value;
                    _neighborhoodMeetingRepository.UpdateScheduledDate(meeting.Id, date);
                    _neighborhoodMeetingRepository.UpdateStatus(meeting.Id, MeetingStatus.Scheduled);
                    break;

                case MeetingOutcome.Tied:
                case MeetingOutcome.TooEarly:
                    break;
            }
        }
    }
}
