using CommunityHub.Application.Domain.NeighborhoodMeetings;
using CommunityHub.Application.Domain.Users;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class MeetingDateVoteMapper : EntityMapper<MeetingDateVote>
    {
        private readonly NeighborhoodMeetingMapper _meetingMapper = new NeighborhoodMeetingMapper();
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();

        public override MeetingDateVote MapToEntity(IDataReader reader)
        {
            NeighborhoodMeeting meeting = _meetingMapper.MapToEntity(reader);
            Citizen citizen = _citizenMapper.MapToEntity(reader);

            long id = reader.GetInt64(reader.GetOrdinal("id"));
            DateOnly votedDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("voted_date")));

            return new MeetingDateVote(meeting, citizen, votedDate, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(MeetingDateVote entity)
        {
            return new Dictionary<string, object?>
            {
                { "voted_date", entity.VotedDate.ToDateTime(TimeOnly.MinValue) },
                { "meeting_id", entity.Meeting.Id },
                { "citizen_id", entity.Citizen.Id }
            };
        }
    }
}
