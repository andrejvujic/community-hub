using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.Services.Apartments;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;

namespace CommunityHub.Application.Services.ResidentAssemblies
{
    public class AssemblyAttendanceService
    {
        private readonly IAssemblyAttendanceRepository _assemblyAttendanceRepository =
            Injector.CreateInstance<IAssemblyAttendanceRepository>();
        private readonly ApartmentMembershipService _apartmentMembershipService = new();

        public bool HasRegisteredAttendance(long assemblyId, long residentId)
            => _assemblyAttendanceRepository.HasRegisteredAttendance(assemblyId, residentId);

        private List<long> GetResidentApartmentIds(AssemblyDto assembly, long residentId)
            => _apartmentMembershipService.GetApartmentIdsByResidentAndBuilding(
                residentId,
                assembly.BuildingId
            );

        public AssemblyDto RegisterAttendance(AssemblyDto assembly, long residentId)
        {
            EnsureAttendanceCanBeChanged(assembly);

            var apartmentIds = GetResidentApartmentIds(assembly, residentId);
            var confirmedApartmentIds = assembly.Attendances
                .Select(attendance => attendance.ApartmentId)
                .ToHashSet();

            foreach (var apartmentId in apartmentIds.Where(id => !confirmedApartmentIds.Contains(id)))
                assembly.Attendances.Add(_assemblyAttendanceRepository.Create(
                    new AssemblyAttendance(assembly.Id, apartmentId)
                ));

            return assembly;
        }

        public AssemblyDto UnregisterAttendance(AssemblyDto assembly, long residentId)
        {
            EnsureAttendanceCanBeChanged(assembly);

            var apartmentIds = GetResidentApartmentIds(assembly, residentId).ToHashSet();
            var residentAttendances = assembly.Attendances
                .Where(attendance => apartmentIds.Contains(attendance.ApartmentId))
                .ToList();

            foreach (var attendance in residentAttendances)
            {
                _assemblyAttendanceRepository.Delete(attendance.Id);
                assembly.Attendances.Remove(attendance);
            }

            return assembly;
        }

        private static void EnsureAttendanceCanBeChanged(AssemblyDto assembly)
        {
            if (assembly.Status is AssemblyStatus.Cancelled || assembly.StartsAt <= DateTime.Now.AddHours(24))
                throw new InvalidOperationException(
                    "Assembly attendance can only be changed until 24 hours before an active assembly starts."
                );
        }
    }
}
