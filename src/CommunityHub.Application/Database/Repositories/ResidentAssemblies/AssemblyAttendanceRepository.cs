using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Database.Repositories.ResidentAssemblies
{
    public class AssemblyAttendanceRepository : IAssemblyAttendanceRepository
    {
        private readonly AssemblyAttendanceMapper _assemblyAttendanceMapper = new();

        public AssemblyAttendance Create(AssemblyAttendance attendance)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery
                .Create(
                    @"INSERT INTO assembly_attendances
                        (assembly_id, apartment_id)
                    VALUES
                        (@assembly_id, @apartment_id)
                    RETURNING id;"
                )
                .ExtendParameters(_assemblyAttendanceMapper.MapToQueryParameters(attendance));

            var id = connection.ExecuteScalar(query, Convert.ToInt64);

            return new AssemblyAttendance(
                assemblyId: attendance.AssemblyId,
                apartmentId: attendance.ApartmentId,
                apartment: attendance.Apartment,
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

        public bool HasRegisteredAttendance(long assemblyId, long residentId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery
                .Create(
                    @"SELECT
                        EXISTS (
                            SELECT 1
                            FROM apartment_memberships membership
                            INNER JOIN apartments apartment ON membership.apartment_id = apartment.id
                            INNER JOIN assemblies assembly ON apartment.building_id = assembly.building_id
                            WHERE assembly.id = @assembly_id
                                AND membership.building_resident_id = @resident_id
                        )
                        AND NOT EXISTS (
                            SELECT 1
                            FROM apartment_memberships membership
                            INNER JOIN apartments apartment ON membership.apartment_id = apartment.id
                            INNER JOIN assemblies assembly ON apartment.building_id = assembly.building_id
                            LEFT JOIN assembly_attendances attendance
                                ON attendance.assembly_id = assembly.id
                                AND attendance.apartment_id = apartment.id
                            WHERE assembly.id = @assembly_id
                                AND membership.building_resident_id = @resident_id
                                AND attendance.id IS NULL
                        );"
                )
                .AddParameter("assembly_id", assemblyId)
                .AddParameter("resident_id", residentId);

            return connection.ExecuteScalar(query, Convert.ToBoolean);
        }
    }
}
