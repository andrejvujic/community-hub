using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class StreetMapper : EntityMapper<Street>
    {
        public override Street MapToEntity(IDataReader reader, string? columnPrefix = null)
        {
            long id = reader.GetInt64(reader.GetOrdinalPrefixed("street_id", columnPrefix));
            string name = reader.GetString(reader.GetOrdinalPrefixed("street_name", columnPrefix));
            return new Street(id, name);
        }

        public override Dictionary<string, object?> MapToQueryParameters(Street entity)
        {
            return new Dictionary<string, object?>
            {
                { "street_name", entity.Name }
            };
        }
    }
}
