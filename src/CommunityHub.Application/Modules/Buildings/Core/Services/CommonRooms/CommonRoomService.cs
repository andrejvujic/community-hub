using CommunityHub.Application.Domain.Buildings.CommonRooms;
using CommunityHub.Application.Domain.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.CommonRooms
{
    public class CommonRoomService
    {
        private readonly ICommonRoomRepository _commonRoomRepository
            = Injector.CreateInstance<ICommonRoomRepository>();

        public List<CommonRoom> GetByBuilding(long buildingId)
            => _commonRoomRepository.GetByBuilding(buildingId);
    }
}
