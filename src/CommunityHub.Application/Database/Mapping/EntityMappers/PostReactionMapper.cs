using CommunityHub.Application.Domain.ServiceForums;
using CommunityHub.Application.Domain.Users;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class PostReactionMapper : EntityMapper<PostReaction>
    {
        private readonly ForumPostMapper _postMapper = new ForumPostMapper();
        private readonly ServiceProviderMapper _providerMapper = new ServiceProviderMapper();

        public override PostReaction? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("reaction_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            ForumPost? post = _postMapper.MapToEntity(reader);
            if (post is null) return null;

            ServiceProvider? reactor = _providerMapper.MapToEntity(reader.WithPrefix("reactor"));
            if (reactor is null) return null;

            bool isUpvote = reader.GetBoolean(reader.GetOrdinal("is_upvote"));

            return new PostReaction(post, reactor, isUpvote, id);
        }

        public override Dictionary<string, object?> MapToQueryParameters(PostReaction entity)
        {
            return new Dictionary<string, object?>
            {
                { "is_upvote", entity.IsUpvote },
                { "post_id", entity.Post.Id },
                { "reactor_id", entity.Reactor.Id }
            };
        }
    }
}