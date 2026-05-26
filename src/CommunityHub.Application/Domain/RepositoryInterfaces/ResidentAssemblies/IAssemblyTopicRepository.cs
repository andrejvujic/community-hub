using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies
{
    public interface IAssemblyTopicRepository
    {
        public AssemblyTopic Create(AssemblyTopic topic);

        public void Delete(long topicId);

        public List<AssemblyTopic> GetAllByAssembly(
            long assemblyId,
            AssemblyTopicStatus? status = null
        );

        public List<AssemblyTopic> GetAllByAssemblyAndAuthor(
            long assemblyId,
            long authorId,
            AssemblyTopicStatus? status = null
        );
    }
}
