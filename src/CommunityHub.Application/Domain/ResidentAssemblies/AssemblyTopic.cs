using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain.ResidentAssemblies
{
    public class AssemblyTopic
    {
        public long Id { get; private set; }

        public long AssemblyId { get; private set; }

        public long AuthorId { get; private set; }

        public User? Author { get; private set; }

        public string Description { get; private set; }

        public AssemblyTopicStatus Status { get; private set; }

        public AssemblyTopic(
            long assemblyId,
            long authorId,
            string description,
            AssemblyTopicStatus status,
            User? author = null,
            long id = 0
        )
        {
            Id = id;
            AssemblyId = assemblyId;
            AuthorId = authorId;
            Author = author;
            Description = description;
            Status = status;
        }

        public static AssemblyTopic SuggestedByResident(long residentId, string description)
        {
            return new AssemblyTopic(
                assemblyId: 0,
                authorId: residentId,
                description: description,
                status: AssemblyTopicStatus.Suggested
            );
        }
    }
}
