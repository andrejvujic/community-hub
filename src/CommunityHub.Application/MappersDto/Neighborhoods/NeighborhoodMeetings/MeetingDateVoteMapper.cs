using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodMeetings
{
    public class MeetingDateVoteMapper : IMapper<MeetingDateVote, MeetingDateVoteDto>
    {
        private readonly NeighborhoodMeetingMapper _neighborhoodMeetingMapper = new();
        private readonly CitizenMapper _citizenMapper = new();

        public MeetingDateVoteDto MapToDto(MeetingDateVote meetingDateVote)
        {
            if (meetingDateVote == null) throw new ArgumentNullException(nameof(meetingDateVote));

            return new MeetingDateVoteDto(
                meetingDateVote.Id,
                meetingDateVote.VotedDate,
                _neighborhoodMeetingMapper.MapToDto(meetingDateVote.Meeting),
                _citizenMapper.MapToDto(meetingDateVote.Citizen)
            );
        }

        public MeetingDateVote MapToDomain(MeetingDateVoteDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new MeetingDateVote(
                _neighborhoodMeetingMapper.MapToDomain(dto.Meeting),
                _citizenMapper.MapToDomain(dto.Citizen),
                dto.VotedDate,
                dto.Id
            );
        }
    }
}
