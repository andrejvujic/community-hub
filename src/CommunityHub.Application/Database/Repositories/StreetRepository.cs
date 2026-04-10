using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain;
using System.Collections.Generic;

namespace CommunityHub.Application.Database.Repositories
{
    public class StreetRepository
    {
        private readonly StreetMapper _streetMapper = new StreetMapper();
        public List<Street> GetByCityId(long id)
        {
            var query = PostgresQuery.Create(
                @"SELECT 
                street.id as street_id, street.name as street_name FROM streets street
                INNER JOIN cities city
                ON street.city_id = city.id
                WHERE city.id = @id"
            ).AddParameter("id", id);
    
            List<Street> streets = new List<Street>();
            query.ExecuteReader(reader => streets.Add(_streetMapper.MapToEntity(reader)));
            return streets;
        }

    }
}
