using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Users;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodMeetings
{
    public class MeetingDateVoteService
    {
        private readonly IMeetingDateVoteRepository _meetingDateVoteRepository
            = Injector.CreateInstance<IMeetingDateVoteRepository>();
        private readonly INeighborhoodMeetingRepository _meetingRepository
            = Injector.CreateInstance<INeighborhoodMeetingRepository>();
        private readonly CitizenService _citizenService = new();
        private readonly CitizenMapper _citizenMapper = new();
        private readonly MeetingDateVoteMapper _voteMapper = new();
        private readonly NeighborhoodMeetingMapper _meetingMapper = new();

        public Dictionary<DateOnly, int> GetVoteCountsForMeeting(long meetingId)
            => _meetingDateVoteRepository.GetVoteCountsForMeeting(meetingId);

        public MeetingDateVoteDto? GetByMeetingIdAndCitizenId(long meetingId, long citizenId)
        {
            var vote = _meetingDateVoteRepository.GetByMeetingIdAndCitizenId(meetingId, citizenId);
            return vote != null ? _voteMapper.MapToDto(vote) : null;
        }

        public MeetingDateVoteDto CreateVote(long meetingId, long citizenId, DateOnly votedDate)
        {
            var meeting = _meetingRepository.GetById(meetingId)!;
            var citizenDto = _citizenService.GetCitizenById(citizenId)!;
            var citizenDomain = _citizenMapper.MapToEntity(citizenDto);

            var vote = new MeetingDateVote(meeting, citizenDomain, votedDate);
            var created = _meetingDateVoteRepository.Create(vote);
            return _voteMapper.MapToDto(created);
        }

        public void UpdateVote(long voteId, DateOnly newDate)
            => _meetingDateVoteRepository.UpdateDate(voteId, newDate);
    }
}
