using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.ServiceForums
{
    public class ProviderForum
    {
        public long Id { get; private set; }
        public ServiceProvider CreatedBy { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsClosed { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<ForumPost> Posts { get; private set; } = new List<ForumPost>();

        public ProviderForum(
            ServiceProvider createdBy,
            string title,
            string description,
            bool isClosed,
            DateTime createdAt,
            long id = 0,
            List<ForumPost>? posts = null
        )
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Naslov foruma ne sme biti prazan.", nameof(title));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Opis foruma ne sme biti prazan.", nameof(description));

            Id = id;
            CreatedBy = createdBy;
            Title = title;
            Description = description;
            IsClosed = isClosed;
            CreatedAt = createdAt;
            Posts = posts ?? new List<ForumPost>();
        }

        public ProviderForum CopyWith(
            long? id = null,
            ServiceProvider? createdBy = null,
            string? title = null,
            string? description = null,
            bool? isClosed = null,
            DateTime? createdAt = null,
            List<ForumPost>? posts = null
        )
        {
            return new ProviderForum(
                createdBy ?? CreatedBy,
                title ?? Title,
                description ?? Description,
                isClosed ?? IsClosed,
                createdAt ?? CreatedAt,
                id ?? Id,
                posts ?? Posts
            );
        }
    }
}