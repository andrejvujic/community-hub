using System.Net.WebSockets;

namespace CommunityHub.Application.Database.Repositories
{
    public class StreetSectionDbRepository
    {
        public int GetStartNumber(long streetId, long cityId)
        {
            var query = PostgresQuery.Create(
                @"SELECT start_number
                  FROM street_sections INNER JOIN
                  streets ON street_sections.street_id =
                  streets.id INNER JOIN cities ON streets.city_id = cities.id
                  WHERE street_id = @streetId AND city_id = @cityId;"
            )
            .AddParameter("streetId", streetId)
            .AddParameter("cityId", cityId);

            return query.ExecuteScalar<int>(value => Convert.ToInt32(value));
        }
        public int GetEndNumber(long streetId, long cityId)
        {
            var query = PostgresQuery.Create(
                @"SELECT end_number
                  FROM street_sections INNER JOIN
                  streets ON street_sections.street_id =
                  streets.id INNER JOIN cities ON streets.city_id = cities.id
                  WHERE street_id = @streetId AND city_id = @cityId;"
            )
            .AddParameter("streetId", streetId)
            .AddParameter("cityId", cityId);

            return query.ExecuteScalar<int>(value => Convert.ToInt32(value));
        }
    }
}
