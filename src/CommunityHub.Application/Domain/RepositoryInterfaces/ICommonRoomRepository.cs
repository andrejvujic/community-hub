using CommunityHub.Application.Domain.Buildings.CommonRooms;

namespace CommunityHub.Application.Domain.RepositoryInterfaces
{
    public interface ICommonRoomRepository
    {
        public CommonRoom Create(CommonRoom commonRoom);
        public List<CommonRoom> GetByBuilding(long buildingId);
    }
}