using System.Data;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class NeighborhoodMapper : EntityMapper<Neighborhood>
    {
        private readonly CityMapper _cityMapper = new CityMapper();
        private readonly CountryMapper _countryMapper = new CountryMapper();
        private readonly UserMapper? _userMapper = new UserMapper();

        public override Neighborhood MapToEntity(IDataReader reader)
        {
            City city = _cityMapper.MapToEntity(reader);
            Country country = _countryMapper.MapToEntity(reader);
            User? coordinator = (_userMapper?.MapToEntity(reader.WithPrefix("coordinator"))) ?? throw new Exception("Couldn't read Neighborhood from database because of missing data");

            long id = reader.GetInt64(reader.GetOrdinal("neighborhood_id"));
            string name = reader.GetString(reader.GetOrdinal("neighborhood_name"));
            string description = reader.GetString(reader.GetOrdinal("description"));
            int budget = reader.GetInt32(reader.GetOrdinal("budget"));

            return new Neighborhood(name, description, budget, city, country, coordinator, id: id);
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
