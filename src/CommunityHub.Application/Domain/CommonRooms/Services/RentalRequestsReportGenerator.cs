using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.RepositoryInterfaces.CommonRooms;

namespace CommunityHub.Application.Domain.CommonRooms.Services
{
    public class RentalRequestsReportGenerator
    {
        private readonly ICommonRoomRepository _commonRoomRepository = Injector.CreateInstance<ICommonRoomRepository>();
        private readonly ICommonRoomRentalRequestRepository _commonRoomRentalRequestRepository = Injector.CreateInstance<ICommonRoomRentalRequestRepository>();

        public List<CommonRoomRentalRequestsReport> GenerateForBuilding(
            long buildingId,
            DateTime from,
            DateTime to
        )
        {
            var commonRooms = _commonRoomRepository.GetAllByBuildingId(buildingId);
            var rentalRequests = _commonRoomRentalRequestRepository.GetAllByBuildingId(
                buildingId, 
                from: from,
                to: to
            );

            var commonRoomsById = new Dictionary<long, CommonRoom>();
            foreach (var commonRoom in commonRooms)
                commonRoomsById[commonRoom.Id] = commonRoom;

            return GroupRentalRequestsByCommonRoom(commonRoomsById, rentalRequests);
        }

         private static List<CommonRoomRentalRequestsReport> GroupRentalRequestsByCommonRoom(
            Dictionary<long, CommonRoom> commonRooms,
            List<CommonRoomRentalRequest> rentalRequests
         )
         {
            var groups = rentalRequests.GroupBy(r => r.CommonRoomId);

            return groups.Select(
                rentalRequests => new CommonRoomRentalRequestsReport(
                    commonRooms[rentalRequests.First().CommonRoomId],
                    rentalRequests.ToList()
                )
            ).ToList();
        }

    }
}
