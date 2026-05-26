using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;

namespace CommunityHub.Application.MappersDto.ResidentAssemblies
{
    public class AssemblyTopicMapper : IMapper<AssemblyTopic, AssemblyTopicDto>
    {
        private readonly Users.UserMapper _authorMapper = new();

        public AssemblyTopicDto MapToDto(AssemblyTopic topic)
        {
            if (topic is null) throw new ArgumentNullException(nameof(topic));

            return new AssemblyTopicDto(
                id: topic.Id,
                assemblyId: topic.AssemblyId,
                authorId: topic.AuthorId,
                description: topic.Description,
                status: topic.Status,
                author: topic.Author is null ? null : _authorMapper.MapToDto(topic.Author)
            );
        }

        public AssemblyTopic MapToEntity(AssemblyTopicDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            return new AssemblyTopic(
                id: dto.Id,
                assemblyId: dto.AssemblyId,
                authorId: dto.AuthorId,
                description: dto.Description,
                status: dto.Status,
                author: dto.Author is null ? null : _authorMapper.MapToEntity(dto.Author)
            );
        }
    }
}
