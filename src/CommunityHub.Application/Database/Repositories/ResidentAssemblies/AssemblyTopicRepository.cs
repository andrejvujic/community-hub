using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Database.Repositories.ResidentAssemblies
{
    public class AssemblyTopicRepository : IAssemblyTopicRepository
    {
        private readonly AssemblyTopicMapper _topicMapper = new();

        public AssemblyTopic Create(AssemblyTopic topic)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO assembly_topics
                    (assembly_id, author_id, description, topic_type)
                VALUES
                    (@assembly_id, @author_id, @description, @topic_type)
                RETURNING id;"
            ).ExtendParameters(_topicMapper.MapToQueryParameters(topic));

            var id = connection.ExecuteScalar(query, Convert.ToInt64);

            return new AssemblyTopic(
                id: id,
                assemblyId: topic.AssemblyId,
                authorId: topic.AuthorId,
                author: topic.Author,
                description: topic.Description,
                status: topic.Status,
                isEnforcedByManager: topic.IsEnforcedByManager
            );
        }

        public void Delete(long topicId)
        {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery.Create(
                    @"DELETE FROM assembly_topics WHERE id = @id;"
                )
                .AddParameter("id", topicId)
            );
        }

        public List<AssemblyTopic> GetAllByAssembly(
            long assemblyId,
            AssemblyTopicStatus? status = null
        )
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT
                    topic.id AS assembly_topic_id,
                    topic.assembly_id AS assembly_topic_assembly_id,
                    topic.author_id AS assembly_topic_author_id,
                    topic.description AS assembly_topic_description,
                    topic.topic_type AS assembly_topic_status,
                    topic.author_id = building.building_manager_id AS assembly_topic_is_enforced_by_manager,
                    author.id AS author_user_id,
                    author.username AS author_username,
                    author.password AS author_password,
                    author.name AS author_name,
                    author.surname AS author_surname,
                    author.user_role AS author_user_role
                FROM assembly_topics topic
                INNER JOIN users author ON topic.author_id = author.id
                INNER JOIN assemblies assembly ON topic.assembly_id = assembly.id
                INNER JOIN buildings building ON assembly.building_id = building.id
                WHERE topic.assembly_id = @assembly_id AND
                    (topic.topic_type = @status OR @status IS NULL)
                ORDER BY author.name, author.surname, topic.id;"
            )
            .AddParameter("assembly_id", assemblyId)
            .AddParameter("status", status);

            List<AssemblyTopic> topics = new();
            connection.ExecuteReader(query, reader =>
            {
                var topic = _topicMapper.MapToEntity(reader);
                if (topic is not null)
                    topics.Add(topic);
            });

            return topics;
        }

        public List<AssemblyTopic> GetAllByAssemblyAndAuthor(
            long assemblyId,
            long authorId,
            AssemblyTopicStatus? status = null
        )
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT
                    topic.id AS assembly_topic_id,
                    topic.assembly_id AS assembly_topic_assembly_id,
                    topic.author_id AS assembly_topic_author_id,
                    topic.description AS assembly_topic_description,
                    topic.topic_type AS assembly_topic_status,
                    topic.author_id = building.building_manager_id AS assembly_topic_is_enforced_by_manager,
                    author.id AS author_user_id,
                    author.username AS author_username,
                    author.password AS author_password,
                    author.name AS author_name,
                    author.surname AS author_surname,
                    author.user_role AS author_user_role
                FROM assembly_topics topic
                INNER JOIN users author ON topic.author_id = author.id
                INNER JOIN assemblies assembly ON topic.assembly_id = assembly.id
                INNER JOIN buildings building ON assembly.building_id = building.id
                WHERE topic.assembly_id = @assembly_id
                AND topic.author_id = @author_id AND
                    (topic.topic_type = @status OR @status IS NULL)
                ORDER BY topic.id;"
            )
                .AddParameter("assembly_id", assemblyId)
                .AddParameter("author_id", authorId)
                .AddParameter("status", status);

            List<AssemblyTopic> topics = new();
            connection.ExecuteReader(query, reader =>
            {
                var topic = _topicMapper.MapToEntity(reader);
                if (topic is not null)
                    topics.Add(topic);
            });

            return topics;
        }
    }
}
