using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.ServiceForums
{
    public class ForumPost
    {
        public long Id { get; private set; }
        public long ForumId { get; private set; }
        public ServiceProvider Author { get; private set; }
        public string Content { get; private set; }
        public DateTime PostedAt { get; private set; }
        public int UpvoteCount { get; private set; }
        public int DownvoteCount { get; private set; }

        public City AuthorCity => Author.City;
        public Country AuthorCountry => Author.Country;

        public ForumPost(
            long forumId,
            ServiceProvider author,
            string content,
            DateTime postedAt,
            long id = 0,
            int upvoteCount = 0,
            int downvoteCount = 0
        )
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Sadržaj komentara ne sme biti prazan.", nameof(content));

            if (upvoteCount < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(upvoteCount), upvoteCount,
                    "Broj pozitivnih reakcija ne sme biti negativan.");

            if (downvoteCount < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(downvoteCount), downvoteCount,
                    "Broj negativnih reakcija ne sme biti negativan.");

            Id = id;
            ForumId = forumId;
            Author = author;
            Content = content;
            PostedAt = postedAt;
            UpvoteCount = upvoteCount;
            DownvoteCount = downvoteCount;
        }

        public ForumPost CopyWith(
            long? id = null,
            long? forumId = null,
            ServiceProvider? author = null,
            string? content = null,
            DateTime? postedAt = null,
            int? upvoteCount = null,
            int? downvoteCount = null
        )
        {
            return new ForumPost(
                forumId ?? ForumId,
                author ?? Author,
                content ?? Content,
                postedAt ?? PostedAt,
                id ?? Id,
                upvoteCount ?? UpvoteCount,
                downvoteCount ?? DownvoteCount
            );
        }
    }
}