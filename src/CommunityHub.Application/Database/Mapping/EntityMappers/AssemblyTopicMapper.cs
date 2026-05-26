using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AssemblyTopicMapper : EntityMapper<AssemblyTopic>
    {
        private readonly UserMapper _authorMapper = new();

        public override AssemblyTopic? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("assembly_topic_id")) return null;

            var idColumnOrdinal = reader.GetOrdinal("assembly_topic_id");
            if (reader.IsDBNull(idColumnOrdinal)) return null;

            var id = reader.GetInt64(idColumnOrdinal);
            var assemblyId = reader.GetInt64(reader.GetOrdinal("assembly_topic_assembly_id"));
            var authorId = reader.GetInt64(reader.GetOrdinal("assembly_topic_author_id"));
            var author = _authorMapper.MapToEntity(reader.WithPrefix("author"));
            var description = reader.GetString(reader.GetOrdinal("assembly_topic_description"));
            var status = (AssemblyTopicStatus)reader.GetValue(reader.GetOrdinal("assembly_topic_status"));

            return new AssemblyTopic(
                assemblyId: assemblyId,
                authorId: authorId,
                description: description,
                status: status,
                author: author,
                id: id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(AssemblyTopic entity)
        {
            return new Dictionary<string, object?>
            {
                { "assembly_id", entity.AssemblyId },
                { "author_id", entity.AuthorId },
                { "description", entity.Description },
                { "topic_type", entity.Status }
            };
        }
    }
}
