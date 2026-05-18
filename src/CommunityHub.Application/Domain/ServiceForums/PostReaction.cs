using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.ServiceForums
{
    public class PostReaction
    {
        public long Id { get; private set; }
        public ForumPost Post { get; private set; }
        public ServiceProvider Reactor { get; private set; }
        public bool IsUpvote { get; private set; }

        public PostReaction(
            ForumPost post,
            ServiceProvider reactor,
            bool isUpvote,
            long id = 0
        )
        {
            ArgumentNullException.ThrowIfNull(post);
            ArgumentNullException.ThrowIfNull(reactor);

            Id = id;
            Post = post;
            Reactor = reactor;
            IsUpvote = isUpvote;
        }

        public PostReaction CopyWith(
            long? id = null,
            ForumPost? post = null,
            ServiceProvider? reactor = null,
            bool? isUpvote = null
        )
        {
            return new PostReaction(
                post ?? Post,
                reactor ?? Reactor,
                isUpvote ?? IsUpvote,
                id ?? Id
            );
        }
    }
}