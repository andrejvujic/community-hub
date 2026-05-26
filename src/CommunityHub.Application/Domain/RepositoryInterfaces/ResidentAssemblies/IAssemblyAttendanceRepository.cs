using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies
{
    public interface IAssemblyAttendanceRepository
    {
        public AssemblyAttendance Create(AssemblyAttendance attendance);

        public void Delete(long attendanceId);
    }
}
