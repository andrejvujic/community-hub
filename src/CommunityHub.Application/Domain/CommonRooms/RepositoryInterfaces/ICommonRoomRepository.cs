using CommunityHub.Application.Domain.CommonRooms;

namespace CommunityHub.Application.Domain.CommonRooms.RepositoryInterfaces
{
    public interface ICommonRoomRepository
    {
        public CommonRoom Create(CommonRoom commonRoom);
        public List<CommonRoom> GetByBuilding(long buildingId);
    }
}