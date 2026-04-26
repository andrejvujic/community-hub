using CommunityHub.Application.Domain.CommonRoom;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CommonRoomMapper : EntityMapper<CommonRoom>
    {
        public override CommonRoom MapToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("common_room_id"));
            long buildingId = reader.GetInt64(reader.GetOrdinal("building_id"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            string description = reader.GetString(reader.GetOrdinal("description"));
            int floorNumber = reader.GetInt32(reader.GetOrdinal("floor_number"));
            RentalType type = (RentalType)reader.GetValue(reader.GetOrdinal("rental_type"));

            return new CommonRoom(
                buildingId,
                name,
                description,
                floorNumber,
                type,
                id
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