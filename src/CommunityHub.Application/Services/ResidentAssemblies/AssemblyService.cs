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

        public AssemblyDto UpdateStatus(AssemblyDto targetAssembly)
        {
            var assembly = MapToEntity(targetAssembly);

            if (!assembly.CanBeConfirmed && !assembly.ShouldBeCancelled) return targetAssembly;

            if (assembly.CanBeConfirmed)
                assembly.Confirm();

            if (assembly.ShouldBeCancelled)
                assembly.Cancel();

            _assemblyRepository.Update(assembly);
            return MapToDto(assembly);
        }
    }
}
