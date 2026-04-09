using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class CountryMapper : EntityMapper<Country>
    {
        public override Country MapToEntity(IDataReader reader, string? columnNamePrefix = null)
        {
            long id = reader.GetInt64(reader.GetOrdinalPrefixed("country_id", columnNamePrefix));
            string name = reader.GetString(reader.GetOrdinalPrefixed("country_name", columnNamePrefix));
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
