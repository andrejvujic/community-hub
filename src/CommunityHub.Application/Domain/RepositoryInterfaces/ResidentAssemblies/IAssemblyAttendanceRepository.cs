using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies
{
    public interface IAssemblyAttendanceRepository
    {
        public AssemblyAttendance Create(long apartmentId, long assemblyId);

        public void Delete(long attendanceId);

        public List<AssemblyAttendance> GetAllByAssembly(long assemblyId);
    }
}
