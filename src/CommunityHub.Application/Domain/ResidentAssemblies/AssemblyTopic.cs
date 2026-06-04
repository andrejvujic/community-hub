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

        public bool IsEnforcedByManager { get; private set; }

        public bool WasSuggestedByResident =>
           Status == AssemblyTopicStatus.Suggested ||
           (Status == AssemblyTopicStatus.Accepted && !IsEnforcedByManager);

        public AssemblyTopic(
            long assemblyId,
            long authorId,
            string description,
            AssemblyTopicStatus status,
            User? author = null,
            bool isEnforcedByManager = false,
            long id = 0
        )
        {
            Id = id;
            AssemblyId = assemblyId;
            AuthorId = authorId;
            Author = author;
            Description = description;
            Status = status;
            IsEnforcedByManager = isEnforcedByManager;
        }

        public static AssemblyTopic SuggestedByResident(
            long assemblyId,
            long residentId,
            string description
        )
        {
            return new AssemblyTopic(
                assemblyId: assemblyId,
                authorId: residentId,
                description: description,
                status: AssemblyTopicStatus.Suggested
            );
        }
    }
}
