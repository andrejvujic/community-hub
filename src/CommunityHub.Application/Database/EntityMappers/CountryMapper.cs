using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class CountryMapper : EntityMapper<Country>
    {
        public override Country MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("country_id"));
            string name = reader.GetString(reader.GetOrdinal("country_name"));
            return new Country(id, name);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Country entity)
        {
            return new Dictionary<string, object?>
            {
                { "country_name", entity.Name }
            };
        }
    }
}
