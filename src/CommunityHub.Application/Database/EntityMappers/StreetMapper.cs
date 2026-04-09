using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.EntityMappers
{
    public class StreetMapper : EntityMapper<Street>
    {
        public override Street MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("street_id"));
            string name = reader.GetString(reader.GetOrdinal("street_name"));
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
