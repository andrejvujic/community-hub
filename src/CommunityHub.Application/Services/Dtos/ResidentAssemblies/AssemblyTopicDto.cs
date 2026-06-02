using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.ResidentAssemblies
{
    public class AssemblyTopicDto
    {
        public long Id { get; set; }

        public long AssemblyId { get; set; }

        public long AuthorId { get; set; }

        public UserDto? Author { get; set; }

        public string Description { get; set; }

        public AssemblyTopicStatus Status { get; set; }

        public bool IsEnforcedByManager { get; set; }

        public bool WasSuggestedByResident { get; set; }

        public AssemblyTopicDto(
            long id,
            long assemblyId,
            long authorId,
            string description,
            AssemblyTopicStatus status,
            UserDto? author = null,
            bool isEnforcedByManager = false,
            bool wasSuggestedByResident = false
        )
        {
            Id = id;
            AssemblyId = assemblyId;
            AuthorId = authorId;
            Author = author;
            Description = description;
            Status = status;
            IsEnforcedByManager = isEnforcedByManager;
            WasSuggestedByResident = wasSuggestedByResident;
        }
    }
}
