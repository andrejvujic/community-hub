namespace CommunityHub.Application.Domain
{
    public class ForumPost
    {
        public long Id { get; private set; }
        public long ForumId { get; private set; }
        public ServiceProvider Author { get; private set; }
        public string Content { get; private set; }
        public City AuthorCity { get; private set; }
        public Country AuthorCountry { get; private set; }
        public DateTime PostedAt { get; private set; }
        public int UpvoteCount { get; private set; }
        public int DownvoteCount { get; private set; }
        public PostReaction? CurrentUserReaction { get; private set; }

        public ForumPost(
            long forumId,
            ServiceProvider author,
            string content,
            City authorCity,
            Country authorCountry,
            DateTime postedAt,
            long id = 0,
            int upvoteCount = 0,
            int downvoteCount = 0,
            PostReaction? currentUserReaction = null
        )
        {
            Id = id;
            ForumId = forumId;
            Author = author;
            Content = content;
            AuthorCity = authorCity;
            AuthorCountry = authorCountry;
            PostedAt = postedAt;
            UpvoteCount = upvoteCount;
            DownvoteCount = downvoteCount;
            CurrentUserReaction = currentUserReaction;
        }

        public ForumPost CopyWith(
            long? id = null,
            long? forumId = null,
            ServiceProvider? author = null,
            string? content = null,
            City? authorCity = null,
            Country? authorCountry = null,
            DateTime? postedAt = null,
            int? upvoteCount = null,
            int? downvoteCount = null,
            PostReaction? currentUserReaction = null,
            bool clearCurrentUserReaction = false
        )
        {
            return new ForumPost(
                forumId ?? ForumId,
                author ?? Author,
                content ?? Content,
                authorCity ?? AuthorCity,
                authorCountry ?? AuthorCountry,
                postedAt ?? PostedAt,
                id ?? Id,
                upvoteCount ?? UpvoteCount,
                downvoteCount ?? DownvoteCount,
                clearCurrentUserReaction ? null : (currentUserReaction ?? CurrentUserReaction)
            );
        }
    }
}