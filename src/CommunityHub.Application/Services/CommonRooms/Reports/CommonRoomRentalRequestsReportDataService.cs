using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.RepositoryInterfaces.CommonRooms;

namespace CommunityHub.Application.Services.CommonRooms.Reports
{
    public class CommonRoomRentalRequestsReportDataService
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
            var reportsByCommonRoomId = rentalRequests
                .GroupBy(r => r.CommonRoomId)
                .Select(
                    group => new CommonRoomRentalRequestsReport(
                        commonRooms[group.Key],
                        group.ToList()
                    )
                )
                .ToDictionary(report => report.CommonRoom.Id);

            return commonRooms
                .Values
                .Select(commonRoom => reportsByCommonRoomId.TryGetValue(commonRoom.Id, out var report)
                    ? report
                    : new CommonRoomRentalRequestsReport(commonRoom, new List<CommonRoomRentalRequest>()))
                .ToList();
        }
    }
}
