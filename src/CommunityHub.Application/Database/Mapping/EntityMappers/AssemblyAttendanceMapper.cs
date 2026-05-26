using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AssemblyAttendanceMapper : EntityMapper<AssemblyAttendance>
    {
        public override AssemblyAttendance? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("assembly_attendance_id")) return null;

            var idColumnOrdinal = reader.GetOrdinal("assembly_attendance_id");
            if (reader.IsDBNull(idColumnOrdinal)) return null;

            var id = reader.GetInt64(idColumnOrdinal);
            var assemblyId = reader.GetInt64(reader.GetOrdinal("assembly_attendance_assembly_id"));
            var apartmentId = reader.GetInt64(reader.GetOrdinal("assembly_attendance_apartment_id"));

            return new AssemblyAttendance(
                assemblyId: assemblyId,
                apartmentId: apartmentId,
                id: id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(AssemblyAttendance entity)
        {
            return new Dictionary<string, object?>
            {
                { "assembly_id", entity.AssemblyId },
                { "apartment_id", entity.ApartmentId }
            };
        }
    }
}
