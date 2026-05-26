using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.MappersDto.ResidentAssemblies;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;

namespace CommunityHub.Application.Services.ResidentAssemblies
{
    public class AssemblyTopicService : BaseService<AssemblyTopic, AssemblyTopicDto>
    {
        private readonly IAssemblyTopicRepository _topicRepository =
            Injector.CreateInstance<IAssemblyTopicRepository>();

        public AssemblyTopicService() : base(new AssemblyTopicMapper()) { }

        public List<AssemblyTopicDto> GetAllByAssembly(
            long assemblyId,
            AssemblyTopicStatus? status = null
        ) => MapToDto(_topicRepository.GetAllByAssembly(assemblyId, status));

        public List<AssemblyTopicDto> GetAllByAssemblyAndAuthor(
            long assemblyId,
            long authorId,
            AssemblyTopicStatus? status = null
        ) => MapToDto(_topicRepository.GetAllByAssemblyAndAuthor(assemblyId, authorId, status));
    }
}
