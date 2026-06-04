using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.RepositoryInterfaces.Addresses;

namespace CommunityHub.Application.Database.Repositories.Shared.Addresses
{
    public class StreetRepository : IStreetRepository
    {
        private readonly StreetMapper _streetMapper = new();
        public List<Street> GetByCityId(long id)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT 
                street.id as street_id, street.name as street_name FROM streets street
                INNER JOIN cities city
                ON street.city_id = city.id
                WHERE city.id = @id"
            ).AddParameter("id", id);
    
            List<Street> streets = new List<Street>();
            connection.ExecuteReader(query, reader => streets.Add(_streetMapper.MapToEntity(reader)));
            return streets;
        }
    }
}