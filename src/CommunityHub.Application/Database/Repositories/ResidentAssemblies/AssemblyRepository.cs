using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Database.Repositories.ResidentAssemblies
{
    public class AssemblyRepository : IAssemblyRepository
    {
        private readonly AssemblyMapper _assemblyMapper = new();
        private readonly AssemblyAttendanceMapper _attendanceMapper = new();

        public Assembly Create(Assembly assembly)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery
                .Create(
                    @"INSERT INTO assemblies
                        (building_id, starts_at, status)
                    VALUES
                        (@building_id, @starts_at, @status)
                    RETURNING id;"
                )
                .ExtendParameters(_assemblyMapper.MapToQueryParameters(assembly));

            var id = connection.ExecuteScalar(query, Convert.ToInt64);

            return new Assembly(
                id: id,
                buildingId: assembly.BuildingId,
                buildingApartmentCount: assembly.BuildingApartmentCount,
                startsAt: assembly.StartsAt,
                status: assembly.Status,
                attendances: assembly.Attendances.ToList()
            );
        }

        public void Update(Assembly assembly)
        {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery
                    .Create(
                        @"UPDATE assemblies
                        SET
                            building_id = @building_id,
                            starts_at = @starts_at,
                            status = @status
                        WHERE id = @id;"
                    )
                    .AddParameter("id", assembly.Id)
                    .ExtendParameters(_assemblyMapper.MapToQueryParameters(assembly))
            );
        }

        public List<Assembly> GetAllByBuilding(long buildingId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery
                .Create(
                    @"SELECT
                        assembly.id AS assembly_id,
                        assembly.building_id AS assembly_building_id,
                        building.apartment_count,
                        assembly.starts_at,
                        assembly.status AS assembly_status,
                        attendance.id AS assembly_attendance_id,
                        attendance.assembly_id AS assembly_attendance_assembly_id,
                        attendance.apartment_id AS assembly_attendance_apartment_id
                    FROM assemblies assembly
                    INNER JOIN buildings building ON assembly.building_id = building.id
                    LEFT JOIN assembly_attendances attendance ON attendance.assembly_id = assembly.id
                    WHERE assembly.building_id = @building_id
                    ORDER BY assembly.starts_at DESC;"
                )
                .AddParameter("building_id", buildingId);

            Dictionary<long, Assembly> assemblies = new();

            connection.ExecuteReader(
                query,
                reader =>
                {
                    var assembly = _assemblyMapper.MapToEntity(reader);
                    if (assembly is null) return;

                    if (!assemblies.TryGetValue(assembly.Id, out var existingAssembly))
                    {
                        assemblies.Add(assembly.Id, assembly);
                        existingAssembly = assembly;
                    }

                    var attendance = _attendanceMapper.MapToEntity(reader);
                    if (attendance is not null)
                        existingAssembly.RegisterAttendance(attendance);
                }
            );

            return assemblies.Values.ToList();
        }

        public Assembly? GetById(long assemblyId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery
                .Create(
                    @"SELECT
                        assembly.id AS assembly_id,
                        assembly.building_id AS assembly_building_id,
                        building.apartment_count,
                        assembly.starts_at,
                        assembly.status AS assembly_status,
                        attendance.id AS assembly_attendance_id,
                        attendance.assembly_id AS assembly_attendance_assembly_id,
                        attendance.apartment_id AS assembly_attendance_apartment_id
                    FROM assemblies assembly
                    INNER JOIN buildings building ON assembly.building_id = building.id
                    LEFT JOIN assembly_attendances attendance ON attendance.assembly_id = assembly.id
                    WHERE assembly.id = @assembly_id;"
                )
                .AddParameter("assembly_id", assemblyId);

            Assembly? assembly = null;

            connection.ExecuteReader(
                query,
                reader =>
                {
                    assembly ??= _assemblyMapper.MapToEntity(reader);
                    if (assembly is null) return;

                    var attendance = _attendanceMapper.MapToEntity(reader);
                    if (attendance is not null)
                        assembly.RegisterAttendance(attendance);
                }
            );

            return assembly;
        }
    }
}
