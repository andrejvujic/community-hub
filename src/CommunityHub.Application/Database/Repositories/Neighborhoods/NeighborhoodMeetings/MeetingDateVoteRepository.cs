using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodMeetings;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodMeetings;

namespace CommunityHub.Application.Database.Repositories.Neighborhoods.NeighborhoodMeetings
{
    public class MeetingDateVoteRepository : IMeetingDateVoteRepository
    {
        private readonly MeetingDateVoteMapper _voteMapper = new MeetingDateVoteMapper();

        public MeetingDateVote Create(MeetingDateVote vote)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO meeting_date_votes (voted_date, meeting_id, citizen_id) 
                VALUES (@voted_date, @meeting_id, @citizen_id) 
                RETURNING id"
            ).ExtendParameters(_voteMapper.MapToQueryParameters(vote));

            long generatedId = connection.ExecuteScalar(query, Convert.ToInt64);

            return vote.CopyWith(id: generatedId);
        }

        public void UpdateDate(long voteId, DateOnly newDate)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                "UPDATE meeting_date_votes SET voted_date = @votedDate WHERE id = @id"
            )
            .AddParameter("votedDate", newDate.ToDateTime(TimeOnly.MinValue))
            .AddParameter("id", voteId);

            connection.GetRowsAffectedCount(query);
        }

        public MeetingDateVote? GetByMeetingIdAndCitizenId(long meetingId, long citizenId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT 
                v.id, v.voted_date, 
        
                m.id AS meeting_id, 
                m.theme AS meeting_theme, 
                m.meeting_time, 
                m.date_range_start, 
                m.date_range_end, 
                m.scheduled_date, 
                m.status AS meeting_status,

                n.id AS neighborhood_id, 
                n.name AS neighborhood_name, 
                n.description, 
                n.budget,
        
                c.id AS user_id, 
                c.username, 
                c.password, 
                c.name, 
                c.surname, 
                c.user_role,
        
                addr.id AS address_id,
                addr.street_number,
                addr.neighborhood,
                addr.street_id,
        
                s.id AS street_id,
                s.name AS street_name,

                city.id AS city_id,
                city.name AS city_name,
                city.country_id,
                country.id AS country_id,
                country.name AS country_name,

                mem.id AS membership_id,
                mem.acceptance_date,

                coord.id AS coordinator_user_id,
                coord.username AS coordinator_username,
                coord.password AS coordinator_password,
                coord.name AS coordinator_name,
                coord.surname AS coordinator_surname,
                coord.user_role AS coordinator_user_role

                FROM meeting_date_votes v
                INNER JOIN neighborhood_meetings m ON v.meeting_id = m.id
                INNER JOIN citizens c ON v.citizen_id = c.id
                INNER JOIN neighborhoods n ON m.neighborhood_id = n.id
                INNER JOIN addresses addr ON c.address_id = addr.id
                INNER JOIN streets s ON addr.street_id = s.id
                INNER JOIN cities city ON addr.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                INNER JOIN users coord ON n.coordinator_id = coord.id
                LEFT JOIN neighborhood_memberships mem ON c.membership_id = mem.id
                WHERE v.meeting_id = @meetingId AND v.citizen_id = @citizenId"
            )
            .AddParameter("meetingId", meetingId)
            .AddParameter("citizenId", citizenId);

            MeetingDateVote? vote = null;
            connection.ExecuteReader(query, reader => vote = _voteMapper.MapToEntity(reader));

            return vote;
        }

        public Dictionary<DateOnly, int> GetVoteCountsForMeeting(long meetingId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT voted_date, COUNT(*) as vote_count 
                FROM meeting_date_votes 
                WHERE meeting_id = @meetingId 
                GROUP BY voted_date"
            ).AddParameter("meetingId", meetingId);

            var voteCounts = new Dictionary<DateOnly, int>();
            connection.ExecuteReader(query, reader =>
            {
                var date = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("voted_date")));
                var count = reader.GetInt32(reader.GetOrdinal("vote_count"));
                voteCounts[date] = count;
            });

            return voteCounts;
        }
    }
}
