using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class CountryMapper : EntityMapper<Country>
    {
        public override Country MapToEntity(IDataReader reader, string? columnPrefix = null)
        {
            long id = reader.GetInt64(reader.GetOrdinalPrefixed("country_id", columnPrefix));
            string name = reader.GetString(reader.GetOrdinalPrefixed("country_name", columnPrefix));
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
