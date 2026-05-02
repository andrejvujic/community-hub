using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class MeetingDateVoteRepository
    {
        private readonly MeetingDateVoteMapper _voteMapper = new MeetingDateVoteMapper();

        public MeetingDateVote Create(MeetingDateVote vote)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO meeting_date_votes (voted_date, meeting_id, citizen_id) 
                VALUES (@voted_date, @meeting_id, @citizen_id) 
                RETURNING id"
            ).ExtendParameters(_voteMapper.MapToQueryParameters(vote));

            long generatedId = query.ExecuteScalar<long>(value => Convert.ToInt64(value));

            return vote.CopyWith(id: generatedId);
        }

        public void UpdateDate(long voteId, DateOnly newDate)
        {
            var query = PostgresQuery.Create(
                "UPDATE meeting_date_votes SET voted_date = @votedDate WHERE id = @id"
            )
            .AddParameter("votedDate", newDate.ToDateTime(TimeOnly.MinValue))
            .AddParameter("id", voteId);

            query.GetRowsAffectedCount();
        }

        public MeetingDateVote? GetByMeetingIdAndCitizenId(long meetingId, long citizenId)
        {
            var query = PostgresQuery.Create(
                @"SELECT 
                v.id, v.voted_date, 
                m.id AS meeting_id, m.theme AS meeting_theme, m.meeting_time, m.date_range_start, m.date_range_end, m.scheduled_date, m.meeting_status,
                c.id AS user_id, c.username, c.password, c.name, c.surname, c.user_role,
                n.id AS neighborhood_id, n.name AS neighborhood_name, n.description, n.budget
                FROM meeting_date_votes v
                INNER JOIN neighborhood_meetings m ON v.meeting_id = m.id
                INNER JOIN citizens c ON v.citizen_id = c.id
                INNER JOIN neighborhoods n ON m.neighborhood_id = n.id
                WHERE v.meeting_id = @meetingId AND v.citizen_id = @citizenId"
            )
            .AddParameter("meetingId", meetingId)
            .AddParameter("citizenId", citizenId);

            MeetingDateVote? vote = null;
            query.ExecuteReader(reader => vote = _voteMapper.MapToEntity(reader));

            return vote;
        }
    }
}
