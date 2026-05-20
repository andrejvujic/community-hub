using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Domain.RepositoryInterfaces.CommonRooms;

namespace CommunityHub.Application.Database.Repositories.CommonRooms
{
    public class CommonRoomRepository : ICommonRoomRepository
    {
        private readonly CommonRoomMapper _commonRoomMapper = new();

        public CommonRoom Create(CommonRoom commonRoom)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"INSERT INTO common_rooms (building_id, name, description, floor_number, rental_type) 
                VALUES (@building_id, @name, @description, @floor_number, @rental_type)
                RETURNING id"
            )
            .ExtendParameters(_commonRoomMapper.MapToQueryParameters(commonRoom));

            long generatedId = connection.ExecuteScalar(query, result => Convert.ToInt64(result));

            return commonRoom.CopyWith(id: generatedId);
        }

        public List<CommonRoom> GetByBuilding(long buildingId)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery.Create(
                @"SELECT 
	                common_room.id AS common_room_id,
	                common_room.building_id,
	                common_room.name,
	                common_room.description,
	                common_room.floor_number,
	                common_room.rental_type

                    FROM common_rooms common_room
                    WHERE common_room.building_id = @building_id"
            )
            .AddParameter("building_id", buildingId);

            List<CommonRoom> commonRooms = [];

            connection.ExecuteReader(query, reader => commonRooms.Add(
                    _commonRoomMapper.MapToEntity(reader)
                )
            );

            return commonRooms;
        }
    }
}