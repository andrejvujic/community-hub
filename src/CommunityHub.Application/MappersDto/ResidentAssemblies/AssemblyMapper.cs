using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;

namespace CommunityHub.Application.MappersDto.ResidentAssemblies
{
    public class AssemblyMapper : IMapper<Assembly, AssemblyDto>
    {
        public AssemblyDto MapToDto(Assembly assembly)
        {
            if (assembly is null) throw new ArgumentNullException(nameof(assembly));

            return new AssemblyDto(
                id: assembly.Id,
                buildingId: assembly.BuildingId,
                buildingApartmentCount: assembly.BuildingApartmentCount,
                startsAt: assembly.StartsAt,
                status: assembly.Status,
                attendances: assembly.Attendances.ToList()
            );
        }

        public Assembly MapToEntity(AssemblyDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            return new Assembly(
                id: dto.Id,
                buildingId: dto.BuildingId,
                buildingApartmentCount: dto.BuildingApartmentCount,
                startsAt: dto.StartsAt,
                status: dto.Status,
                attendances: dto.Attendances.ToList()
            );
        }
    }
}
