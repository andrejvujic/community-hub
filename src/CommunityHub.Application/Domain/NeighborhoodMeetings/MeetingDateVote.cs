using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.NeighborhoodMeetings
{
    public class MeetingDateVote
    {
        public long Id { get; private set; }
        public DateOnly VotedDate { get; private set; }
        public NeighborhoodMeeting Meeting { get; private set; }
        public Citizen Citizen { get; private set; }

        public MeetingDateVote(
            NeighborhoodMeeting meeting,
            Citizen citizen,
            DateOnly votedDate,
            long id = 0
        )
        {
            Id = id;
            Meeting = meeting;
            Citizen = citizen;
            VotedDate = votedDate;
        }

        public MeetingDateVote CopyWith(
            long? id = null,
            NeighborhoodMeeting? meeting = null,
            Citizen? citizen = null,
            DateOnly? votedDate = null
        )
        {
            return new MeetingDateVote(
                meeting ?? Meeting,
                citizen ?? Citizen,
                votedDate ?? VotedDate,
                id ?? Id
            );
        }
    }
}
