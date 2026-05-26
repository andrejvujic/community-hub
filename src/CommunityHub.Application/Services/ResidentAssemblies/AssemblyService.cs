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
    }
}
