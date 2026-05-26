using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.MappersDto.ResidentAssemblies;
using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;
using System.Diagnostics;

namespace CommunityHub.Application.Services.ResidentAssemblies
{
    public class AssemblyAttendanceService
    {
        private readonly IAssemblyAttendanceRepository _assemblyAttendanceRepository =
            Injector.CreateInstance<IAssemblyAttendanceRepository>();

        private readonly AssemblyService _assemblyService = new();

        private readonly ApartmentMembershipService _apartmentMembershipService = new();

        private readonly AssemblyMapper _assemblyMapper = new();

        public bool HasRegisteredAttendance(long assemblyId, long residentId)
        {
            var assembly = _assemblyService.GetById(assemblyId);
            if (assembly is null) return false;

            var attendanceApartmentIds = assembly.Attendances.Select(
                attendance => attendance.ApartmentId
            ).ToHashSet();

            var residentApartmentIds = _apartmentMembershipService.GetApartmentIdsByResidentAndBuilding(
                residentId,
                assembly.BuildingId
            );

            return residentApartmentIds.All(attendanceApartmentIds.Contains);
        }

        public AssemblyDto RegisterAttendance(AssemblyDto targetAssembly, long residentId)
        {
          
            EnsureAttendanceCanBeChanged(targetAssembly);

            var assembly = _assemblyMapper.MapToEntity(targetAssembly);

            var residentApartmentIds = _apartmentMembershipService.GetApartmentIdsByResidentAndBuilding(
                residentId,
                assembly.BuildingId
            );

            foreach (var apartmentId in residentApartmentIds)
            {
                if (assembly.HasRegisteredAttendance(apartmentId)) continue;
                var attendance = _assemblyAttendanceRepository.Create(apartmentId, assembly.Id);
                assembly.RegisterAttendance(attendance);
            }

            return _assemblyService.RefreshStatus(
                _assemblyMapper.MapToDto(assembly)    
            );
        }

        public AssemblyDto UnregisterAttendance(AssemblyDto targetAssembly, long residentId)
        {
            EnsureAttendanceCanBeChanged(targetAssembly);

            var assembly = _assemblyMapper.MapToEntity(targetAssembly);

            var residentApartmentIds = _apartmentMembershipService.GetApartmentIdsByResidentAndBuilding(
                residentId,
                assembly.BuildingId
            );

            var residentAttendances = assembly.Attendances
                .Where(attendance => residentApartmentIds.Contains(attendance.ApartmentId))
                .ToList();

            foreach (var attendance in residentAttendances)
            {
                assembly.UnregisterAttendance(attendance);
                _assemblyAttendanceRepository.Delete(attendance.Id);
            }

            return _assemblyService.RefreshStatus(
                _assemblyMapper.MapToDto(assembly)    
            );
        }

        private static void EnsureAttendanceCanBeChanged(AssemblyDto assembly)
        {
            if (assembly.Status is AssemblyStatus.Cancelled || assembly.StartsAt <= DateTime.Now.AddHours(24))
                throw new InvalidOperationException(
                    "Assembly attendance can only be changed until 24 hours before an active targetAssembly starts."
                );
        }
    }
}
