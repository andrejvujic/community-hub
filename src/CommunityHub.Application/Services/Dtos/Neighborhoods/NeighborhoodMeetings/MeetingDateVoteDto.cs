using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodMeetings
{
    public class MeetingDateVoteDto
    {
        public long Id { get; set; }
        public DateOnly VotedDate { get; set; }
        public NeighborhoodMeetingDto Meeting { get; set; }
        public CitizenDto Citizen { get; set; }

        public MeetingDateVoteDto(
            long id,
            DateOnly votedDate,
            NeighborhoodMeetingDto meeting,
            CitizenDto citizen
        )
        {
            Id = id;
            VotedDate = votedDate;
            Meeting = meeting;
            Citizen = citizen;
        }
    }
}
