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

        private readonly IAssemblyRepository _assemblyRepository =
            Injector.CreateInstance<IAssemblyRepository>();

        public AssemblyTopicService() : base(new AssemblyTopicMapper()) { }

        public AssemblyTopicDto SuggestTopic(long assemblyId, long residentId, string description)
            => MapToDto(
                _topicRepository.Create(
                    AssemblyTopic.SuggestedByResident(
                        assemblyId,
                        residentId,
                        description.Trim()
                    )
                )
            );

        public bool CanEditTopics(long assemblyId)
        {
            var assembly = _assemblyRepository.GetById(assemblyId);
            return assembly is not null && assembly.GetHoursLeftUntilStart() > 24;
        }

        public void DeleteTopic(AssemblyTopicDto topic)
            => _topicRepository.Delete(topic.Id);

        public List<AssemblyTopicDto> GetAllByAssembly(
            long assemblyId,
            AssemblyTopicStatus? status = null
        ) => MapToDto(
            _topicRepository.GetAllByAssembly(assemblyId, status)
        );

        public List<AssemblyTopicDto> GetAllByAssemblyAndAuthor(
            long assemblyId,
            long authorId,
            AssemblyTopicStatus? status = null
        ) => MapToDto(
            _topicRepository.GetAllByAssemblyAndAuthor(assemblyId, authorId, status)
        );
    }
}
