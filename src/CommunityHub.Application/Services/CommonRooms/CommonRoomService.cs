using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.CommonRooms.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;

namespace CommunityHub.Application.Services.CommonRooms
{
    public class CommonRoomService
    {
        private readonly ICommonRoomRepository _commonRoomRepository
            = Injector.CreateInstance<ICommonRoomRepository>();

        private readonly CommonRoomMapper _commonRoomMapper = new();

        public List<CommonRoom> GetByBuilding(long buildingId)
            => _commonRoomRepository.GetByBuilding(buildingId);

        public List<CommonRoomDto> GetAllByBuilding(long buildingId)
        {
            var commonRooms = _commonRoomRepository.GetByBuilding(buildingId);

            return commonRooms.Select(_commonRoomMapper.MapToDto).ToList();
        }
        public void Create(long buildingId, string name, string description, int floorNumber, int rentalTypeIndex)
        {
            var commonRoom = new CommonRoom(
                buildingId: buildingId,
                name: name.Trim(),
                description: description.Trim(),
                floorNumber: floorNumber,
                rentalType: MapRentalType(rentalTypeIndex)
            );

            _commonRoomRepository.Create(commonRoom);
        }
        private RentalType MapRentalType(int rentalTypeIndex)
        {
            return rentalTypeIndex switch
            {
                0 => RentalType.OneDay,
                1 => RentalType.MultiDay,
                _ => throw new ArgumentOutOfRangeException(nameof(rentalTypeIndex), "Invalid rental type index.")
            };
        }
    }
}
