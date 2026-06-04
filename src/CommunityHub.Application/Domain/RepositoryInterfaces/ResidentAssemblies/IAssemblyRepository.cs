using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies
{
    public interface IAssemblyRepository
    {
        public Assembly Create(Assembly assembly);

        public void Update(Assembly assembly);
        public void UpdateMany(List<Assembly> assemblies);

        public List<Assembly> GetAll();
        public List<Assembly> GetAllByBuilding(long buildingId);

        public Assembly? GetById(long assemblyId);
    }
}
