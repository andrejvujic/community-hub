using CommunityHub.Application.Domain.CommonRooms;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.CommonRooms
{
    public interface ICommonRoomRepository
    {
        public CommonRoom Create(CommonRoom commonRoom);
        public List<CommonRoom> GetAllByBuildingId(long buildingId);
    }
}