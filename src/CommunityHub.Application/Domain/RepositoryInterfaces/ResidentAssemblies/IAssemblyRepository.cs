using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies
{
    public interface IAssemblyRepository
    {
        public Assembly Create(Assembly assembly);

        public void Update(Assembly assembly);
    }
}
