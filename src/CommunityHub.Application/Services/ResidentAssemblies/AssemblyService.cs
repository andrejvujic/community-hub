using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.MappersDto.ResidentAssemblies;
using CommunityHub.Application.Services.Buildings;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;

namespace CommunityHub.Application.Services.ResidentAssemblies
{
    public class AssemblyService : BaseService<Assembly, AssemblyDto>
    {
        private readonly IAssemblyRepository _assemblyRepository = Injector.CreateInstance<IAssemblyRepository>();
        private readonly BuildingService _buildingService = new();
        private readonly TimeSpan _assemblyDuration = TimeSpan.FromHours(2);

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

        public void RefreshStatusForAllAssemblies()
        {
            var assemblies = _assemblyRepository.GetAll();

            foreach (var assembly in assemblies)
                assembly.RefreshStatus();

            _assemblyRepository.UpdateMany(assemblies);
        }

        public bool HasOverlappingAssembliesForManager(long managerId, DateTime scheduledAt)
        {
            DateTime newStart = scheduledAt;
            DateTime newEnd = scheduledAt.Add(_assemblyDuration);

            var managerBuildings = _buildingService.GetByManagerId(managerId);

            var assemblies = managerBuildings
                .SelectMany(building => GetAllByBuilding(building.Id))
                .Where(assembly =>
                   assembly.Status == AssemblyStatus.Scheduled ||
                   assembly.Status == AssemblyStatus.Confirmed)
                .ToList();

            return assemblies.Any(assembly =>
            {
                DateTime existingStart = assembly.StartsAt;
                DateTime existingEnd = assembly.StartsAt.Add(_assemblyDuration);

                return newStart < existingEnd && newEnd > existingStart;
            });
        }

        public AssemblyDto Create(AssemblyDto assembly)
        {
            return MapToDto(_assemblyRepository.Create(MapToEntity(assembly)));
        }

        public AssemblyDto? GetNearestAssembly(long managerId)
        {
            var managerBuildings = _buildingService.GetByManagerId(managerId);


            var assembly = managerBuildings
               .SelectMany(building => GetAllByBuilding(building.Id))
               .Where(assembly => assembly.Status == AssemblyStatus.Confirmed || assembly.Status == AssemblyStatus.Scheduled)
               .Where(assembly => assembly.StartsAt >= DateTime.Now)
               .OrderBy(assembly => assembly.StartsAt)
               .FirstOrDefault();

            return assembly;
        }
    }
}