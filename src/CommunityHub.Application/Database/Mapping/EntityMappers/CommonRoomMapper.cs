using CommunityHub.Application.Domain.Buildings.CommonRooms;
using CommunityHub.Application.Domain.Buildings.CommonRooms.RentalRequests;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CommonRoomMapper : EntityMapper<CommonRoom>
    {
        public override CommonRoom? MapToEntity(IDataReader reader)
        {
            if (!reader.HasColumn("common_room_id")) return null;

            var idColumnOrdinal = reader.GetOrdinal("common_room_id");
            if (reader.IsDBNull(idColumnOrdinal)) return null;

            var id = reader.GetInt64(idColumnOrdinal);

            var buildingId = reader.GetInt64(reader.GetOrdinal("building_id"));
            var name = reader.GetString(reader.GetOrdinal("name"));
            var description = reader.GetString(reader.GetOrdinal("description"));
            var floorNumber = reader.GetInt32(reader.GetOrdinal("floor_number"));
            var rentalType = (RentalType)reader.GetValue(reader.GetOrdinal("rental_type"));

            return new CommonRoom(
                id: id,
                buildingId: buildingId,
                name: name,
                description: description,
                floorNumber: floorNumber,
                rentalType: rentalType
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(CommonRoom entity)
        {
            return new Dictionary<string, object?>
            {
                { "building_id", entity.BuildingId },
                { "name", entity.Name },
                { "description", entity.Description },
                { "floor_number", entity.FloorNumber },
                { "rental_type", entity.RentalType }
            };
        }
    }
}