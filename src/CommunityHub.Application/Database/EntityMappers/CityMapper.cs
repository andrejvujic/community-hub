using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class CityMapper : EntityMapper<City>
    {
        public override City MapToEntity(IDataReader reader, string? columnNamePrefix = null)
        {
            long id = reader.GetInt64(reader.GetOrdinalPrefixed("city_id", columnNamePrefix));
            string name = reader.GetString(reader.GetOrdinalPrefixed("city_name", columnNamePrefix));
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
