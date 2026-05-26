using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.MappersDto.ResidentAssemblies
{
    public class AssemblyTopic
    {
        public long Id { get; private set; }

        public long AssemblyId { get; private set; }

        public long AuthorId { get; private set; }
        public User? Author { get; private set; }

        public string Description { get; private set; } = string.Empty;

        public AssemblyTopicStatus Status { get; private set; }

        public static AssemblyTopic SuggestedByResident(long residentId, string description)
        {
            return new AssemblyTopic
            {
                Description = description,
                AuthorId = residentId,
                Status = AssemblyTopicStatus.Suggested
            };
        }
    }
}
