using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Diagnostics;

namespace CommunityHub.Application.Database.Repositories.ResidentAssemblies
{
    public class AssemblyAttendanceRepository : IAssemblyAttendanceRepository
    {
        private readonly AssemblyAttendanceMapper _assemblyAttendanceMapper = new();

        public AssemblyAttendance Create(long apartmentId, long assemblyId)
        {
            using var connection = DatabaseConnection.Create();

            Debug.WriteLine($"Creating assembly attendance for apartmentId: {apartmentId}, assemblyId: {assemblyId}");

            var query = DatabaseQuery
                .Create(
                    @"INSERT INTO assembly_attendances
                        (assembly_id, apartment_id)
                    VALUES
                        (@assembly_id, @apartment_id)
                    RETURNING id;"
                )
                .AddParameter("apartment_id", apartmentId)
                .AddParameter("assembly_id", assemblyId);
                
            var id = connection.ExecuteScalar(query, Convert.ToInt64);

            return new AssemblyAttendance(
                assemblyId: assemblyId,
                apartmentId: apartmentId,
                id: id
            );
        }

        public void Delete(long attendanceId)
        {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery
                    .Create(
                        @"DELETE FROM assembly_attendances
                        WHERE id = @id;"
                    )
                    .AddParameter("id", attendanceId)
            );
        }

        public List<AssemblyAttendance> GetAllByAssembly(long assemblyId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery
                .Create(
                    @"SELECT
                        attendance.id AS assembly_attendance_id,
                        attendance.assembly_id AS assembly_attendance_assembly_id,
                        attendance.apartment_id AS assembly_attendance_apartment_id
                    FROM assembly_attendances attendance
                    WHERE attendance.assembly_id = @assembly_id;"
                )
                .AddParameter("assembly_id", assemblyId);

            List<AssemblyAttendance> attendances = new();

            connection.ExecuteReader(
                query,
                reader =>
                {
                    var attendance = _assemblyAttendanceMapper.MapToEntity(reader);
                    if (attendance is not null)
                        attendances.Add(attendance);
                }
            );

            return attendances;
        }
    }
}
