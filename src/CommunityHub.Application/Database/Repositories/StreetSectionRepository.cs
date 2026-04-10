
using System.Net.WebSockets;

namespace CommunityHub.Application.Database.Repositories
{
    public class StreetSectionRepository
    {
        public int GetStartNumberByStreetId(long streetId)
        {
            var query = PostgresQuery.Create(
                @"SELECT start_number FROM street_sections WHERE street_id = @streetId"
            ).AddParameter("streetId", streetId);

            return query.ExecuteScalar<int>(value => Convert.ToInt32(value));
        }
        public int GetEndNumberByStreetId(long streetId)
        {
            var query = PostgresQuery.Create(
                @"SELECT end_number FROM street_sections WHERE street_id = @streetId"
            ).AddParameter("streetId", streetId);

            return query.ExecuteScalar<int>(value => Convert.ToInt32(value));
        }




    }
}
