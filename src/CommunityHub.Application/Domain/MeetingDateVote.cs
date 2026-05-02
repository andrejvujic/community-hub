namespace CommunityHub.Application.Domain
{
    public class MeetingDateVote
    {
        public long Id { get; private set; }
        public DateTime VotedDate { get; private set; }
        public NeighborhoodMeeting Meeting { get; private set; }
        public Citizen Citizen { get; private set; }

        public MeetingDateVote(
            NeighborhoodMeeting meeting,
            Citizen citizen,
            DateTime votedDate,
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
            DateTime? votedDate = null
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
