using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;

namespace CommunityHub.Application.MappersDto.CommonRooms
{
    public class CommonRoomMapper : IMapper<CommonRoom, CommonRoomDto>
    {
        public CommonRoomDto MapToDto(CommonRoom commonRoom)
        {
            if (commonRoom == null) throw new ArgumentNullException(nameof(commonRoom));

            return new CommonRoomDto(
                commonRoom.Id,
                commonRoom.BuildingId,
                commonRoom.Name,
                commonRoom.Description,
                commonRoom.FloorNumber,
                commonRoom.RentalType
            );
        }
        public CommonRoom MapToEntity(CommonRoomDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new CommonRoom(
                dto.BuildingId,
                dto.Name,
                dto.Description,
                dto.FloorNumber,
                dto.RentalType,
                dto.Id
            );
        }
    }
}