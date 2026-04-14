using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ProviderForumMapper : EntityMapper<ProviderForum>
    {
        private readonly ServiceProviderMapper _providerMapper = new ServiceProviderMapper();

        public override ProviderForum? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("forum_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            ServiceProvider? createdBy = _providerMapper.MapToEntity(reader);
            if (createdBy is null) return null;

            string title = reader.GetString(reader.GetOrdinal("forum_title"));
            string description = reader.GetString(reader.GetOrdinal("forum_description"));
            bool isClosed = reader.GetBoolean(reader.GetOrdinal("is_closed"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("created_at"));

            return new ProviderForum(
                createdBy,
                title,
                description,
                isClosed,
                createdAt,
                id: id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(ProviderForum entity)
        {
            return new Dictionary<string, object?>
            {
                { "title", entity.Title },
                { "description", entity.Description },
                { "is_closed", entity.IsClosed },
                { "created_at", entity.CreatedAt },
                { "created_by_id", entity.CreatedBy.Id }
            };
        }
    }
}