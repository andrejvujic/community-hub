using System.Net.WebSockets;
using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class StreetSectionRepository
    {
        private readonly StreetSectionMapper _streetSectionMapper = new StreetSectionMapper();
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

        public long Create(StreetSection section, long neighborhoodId)
        {
            var query = PostgresQuery.Create(
                @"INSERT INTO street_sections 
                (start_number, end_number, side, street_id, neighborhood_id) 
                VALUES 
                (@start_number, @end_number, @side, @street_id, @neighborhood_id)
                RETURNING id"
            )
            .ExtendParameters(_streetSectionMapper.MapToQueryParameters(section))
            .AddParameter("neighborhood_id", neighborhoodId);

            return query.ExecuteScalar<long>(value => Convert.ToInt64(value));
        }
    }
}
