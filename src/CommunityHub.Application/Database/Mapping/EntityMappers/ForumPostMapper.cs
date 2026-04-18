using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class ForumPostMapper : EntityMapper<ForumPost>
    {
        private readonly ServiceProviderMapper _providerMapper = new ServiceProviderMapper();

        public override ForumPost? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("post_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            long forumId = reader.GetInt64(reader.GetOrdinal("post_forum_id"));

            ServiceProvider? author = _providerMapper.MapToEntity(reader);
            if (author is null) return null;

            string content = reader.GetString(reader.GetOrdinal("post_content"));
            DateTime postedAt = reader.GetDateTime(reader.GetOrdinal("posted_at"));

            int upvoteOrdinal = reader.GetOrdinal("upvote_count");
            int downvoteOrdinal = reader.GetOrdinal("downvote_count");
            int upvoteCount = reader.IsDBNull(upvoteOrdinal) ? 0 : reader.GetInt32(upvoteOrdinal);
            int downvoteCount = reader.IsDBNull(downvoteOrdinal) ? 0 : reader.GetInt32(downvoteOrdinal);

            return new ForumPost(
                forumId,
                author,
                content,
                postedAt,
                id: id,
                upvoteCount: upvoteCount,
                downvoteCount: downvoteCount
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(ForumPost entity)
        {
            return new Dictionary<string, object?>
            {
                { "content", entity.Content },
                { "forum_id", entity.ForumId },
                { "author_id", entity.Author.Id }
            };
        }
    }
}