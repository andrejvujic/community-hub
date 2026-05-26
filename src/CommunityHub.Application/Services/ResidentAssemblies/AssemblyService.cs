using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.MappersDto.ResidentAssemblies;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;

namespace CommunityHub.Application.Services.ResidentAssemblies
{
    public class AssemblyService : BaseService<Assembly, AssemblyDto>
    {
        private readonly IAssemblyRepository _assemblyRepository = Injector.CreateInstance<IAssemblyRepository>();

        public AssemblyService() : base(new AssemblyMapper()) { }

        public List<AssemblyDto> GetAllByBuilding(long buildingId)
            => MapToDto(_assemblyRepository.GetAllByBuilding(buildingId));

        public AssemblyDto? GetById(long assemblyId)
            => _assemblyRepository.GetById(assemblyId) is Assembly assembly
                ? MapToDto(assembly)
                : null;

        public AssemblyDto SynchronizeStatus(AssemblyDto targetAssembly)
        {
            var assembly = MapToEntity(targetAssembly);

            assembly.SynchronizeStatus();
            _assemblyRepository.Update(assembly);

            return MapToDto(assembly);
        }

        public void SynchronizeStatus()
        {
            var assemblies = _assemblyRepository.GetAll();

            foreach (var assembly in assemblies)
                assembly.SynchronizeStatus();

            _assemblyRepository.UpdateMany(assemblies);
        }
    }
}
