using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CityMapper : EntityMapper<City>
    {
        public override City MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("city_id"));
            string name = reader.GetString(reader.GetOrdinal("city_name"));
            return new City(id, name);
        }

        public override Dictionary<string, object?> MapToQueryParameters(City entity)
        {
            return new Dictionary<string, object?>
            {
                { "city_name", entity.Name }
            };
        }
    }
}
