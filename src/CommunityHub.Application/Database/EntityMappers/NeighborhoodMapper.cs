using System.Data;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class NeighborhoodMapper : EntityMapper<Neighborhood>
    {
        private readonly CityMapper _cityMapper = new CityMapper();
        private readonly UserMapper _userMapper = new UserMapper();

        public override Neighborhood MapToEntity(IDataReader reader, string? columnNamePrefix = null)
        {
            City city = _cityMapper.MapToEntity(reader);
            User coordinator = _userMapper.MapToEntity(reader, columnNamePrefix: "coordinator");

            long id = reader.GetInt64(reader.GetOrdinalPrefixed("neighborhood_id", columnNamePrefix));
            string name = reader.GetString(reader.GetOrdinalPrefixed("neighborhood_name", columnNamePrefix));
            string description = reader.GetString(reader.GetOrdinalPrefixed("description", columnNamePrefix));
            int budget = reader.GetInt32(reader.GetOrdinalPrefixed("budget", columnNamePrefix));

            return new Neighborhood(name, description, budget, city, coordinator, id: id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Neighborhood entity)
        {
            return new Dictionary<string, object?>
            {
                { "name", entity.Name },
                { "description", entity.Description },
                { "budget", entity.Budget },
                { "city_id", entity.City.Id },
                { "coordinator_id", entity.Coordinator.Id }
            };
        }
    }
}
