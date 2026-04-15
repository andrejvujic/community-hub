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

            long authorCityId = reader.GetInt64(reader.GetOrdinal("post_city_id"));
            string authorCityName = reader.GetString(reader.GetOrdinal("post_city_name"));
            City authorCity = new City(authorCityId, authorCityName);

            long authorCountryId = reader.GetInt64(reader.GetOrdinal("post_country_id"));
            string authorCountryName = reader.GetString(reader.GetOrdinal("post_country_name"));
            Country authorCountry = new Country(authorCountryId, authorCountryName);

            int upvoteOrdinal = reader.GetOrdinal("upvote_count");
            int downvoteOrdinal = reader.GetOrdinal("downvote_count");
            int upvoteCount = reader.IsDBNull(upvoteOrdinal) ? 0 : reader.GetInt32(upvoteOrdinal);
            int downvoteCount = reader.IsDBNull(downvoteOrdinal) ? 0 : reader.GetInt32(downvoteOrdinal);

            int reactionOrdinal = reader.HasColumn("current_user_reaction")
                ? reader.GetOrdinal("current_user_reaction")
                : -1;
            PostReaction? currentUserReaction = null;
            if (reactionOrdinal >= 0 && !reader.IsDBNull(reactionOrdinal))
            {
                bool isUpvote = reader.GetBoolean(reactionOrdinal);
                currentUserReaction = new PostReaction(
                    post: null!,
                    reactor: author,
                    isUpvote: isUpvote
                );
            }

            return new ForumPost(
                forumId,
                author,
                content,
                authorCity,
                authorCountry,
                postedAt,
                id: id,
                upvoteCount: upvoteCount,
                downvoteCount: downvoteCount,
                currentUserReaction: currentUserReaction
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(ForumPost entity)
        {
            return new Dictionary<string, object?>
            {
                { "content", entity.Content },
                { "posted_at", entity.PostedAt },
                { "forum_id", entity.ForumId },
                { "author_id", entity.Author.Id },
                { "author_city_id", entity.AuthorCity.Id },
                { "author_country_id", entity.AuthorCountry.Id }
            };
        }
    }
}