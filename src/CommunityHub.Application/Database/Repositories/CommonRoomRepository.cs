using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.CommonRoom;

namespace CommunityHub.Application.Database.Repositories
{
    public class CommonRoomRepository
    {
        private readonly CommonRoomMapper _commonRoomMapper = new();

        public List<CommonRoom> GetByUserAndBuilding(long managerId, long buildingId)
        {
            var query = PostgresQuery.Create(
                @"SELECT 
	                common_room.id as common_room_id,
	                common_room.building_id,
	                common_room.name,
	                common_room.description,
	                common_room.floor_number,
	                common_room.rental_type

                    FROM common_rooms common_room
                    INNER JOIN buildings building ON
                    common_room.building_id = building.id
                    WHERE building.building_manager_id = @manager_id
                    AND building.id = @building_id;"
            )
            .AddParameter("manager_id", managerId)
            .AddParameter("building_id", buildingId);

            List<CommonRoom> commonRooms = new();
            query.ExecuteReader(
                reader => commonRooms.Add(_commonRoomMapper.MapToEntity(reader))
            );

            return commonRooms;
        }
    }
}