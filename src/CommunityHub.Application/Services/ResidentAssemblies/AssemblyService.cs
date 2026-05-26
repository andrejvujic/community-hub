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

        public AssemblyDto RefreshStatus(AssemblyDto targetAssembly)
        {
            var assembly = MapToEntity(targetAssembly);

            assembly.RefreshStatus();
            _assemblyRepository.Update(assembly);

            return MapToDto(assembly);
        }

        public void RefreshStatuses()
        {
            var assemblies = _assemblyRepository.GetAll();

            foreach (var assembly in assemblies)
                assembly.RefreshStatus();

            _assemblyRepository.UpdateMany(assemblies);
        }
    }
}
