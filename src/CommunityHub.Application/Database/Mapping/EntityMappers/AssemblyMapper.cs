using CommunityHub.Application.Domain.ResidentAssemblies;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class AssemblyMapper : EntityMapper<Assembly>
    {
        public override Assembly? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("assembly_id")) return null;

            var idColumnOrdinal = reader.GetOrdinal("assembly_id");
            if (reader.IsDBNull(idColumnOrdinal)) return null;

            var id = reader.GetInt64(idColumnOrdinal);
            var buildingId = reader.GetInt64(reader.GetOrdinal("assembly_building_id"));
            var buildingApartmentCount = reader.GetInt32(reader.GetOrdinal("apartment_count"));
            var startsAt = reader.GetDateTime(reader.GetOrdinal("starts_at"));
            var status = (AssemblyStatus)reader.GetValue(reader.GetOrdinal("assembly_status"));

            return new Assembly(
                id: id,
                buildingId: buildingId,
                buildingApartmentCount: buildingApartmentCount,
                startsAt: startsAt,
                status: status
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(Assembly entity)
        {
            return new Dictionary<string, object?>
            {
                { "building_id", entity.BuildingId },
                { "starts_at", entity.StartsAt },
                { "status", entity.Status }
            };
        }
    }
}
