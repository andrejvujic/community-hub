using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Domain.CommonRooms.RepositoryInterfaces;

namespace CommunityHub.Application.Services.CommonRooms
{
    public class CommonRoomService
    {
        private readonly ICommonRoomRepository _commonRoomRepository
            = Injector.CreateInstance<ICommonRoomRepository>();

        public List<CommonRoom> GetByBuilding(long buildingId)
            => _commonRoomRepository.GetByBuilding(buildingId);
    }
}
