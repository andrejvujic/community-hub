using CommunityHub.Application.Domain.CommonRooms;
using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.RepositoryInterfaces.CommonRooms;
using CommunityHub.Application.MappersDto.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms.Reports;

namespace CommunityHub.Application.Services.CommonRooms.Reports
{
    public class CommonRoomRentalRequestsReportDataService
    {
        private readonly ICommonRoomRepository _commonRoomRepository = Injector.CreateInstance<ICommonRoomRepository>();
        private readonly ICommonRoomRentalRequestRepository _commonRoomRentalRequestRepository = Injector.CreateInstance<ICommonRoomRentalRequestRepository>();
        private readonly CommonRoomMapper _commonRoomMapper = new();
        private readonly CommonRoomRentalRequestMapper _rentalRequestMapper = new();

        public List<CommonRoomRentalRequestsReportDto> GenerateForBuilding(
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

            var commonRoomsById = commonRooms.ToDictionary(commonRoom => commonRoom.Id);

            return GroupRentalRequestsByCommonRoom(commonRoomsById, rentalRequests);
        }

        private List<CommonRoomRentalRequestsReportDto> GroupRentalRequestsByCommonRoom(
            Dictionary<long, CommonRoom> commonRooms,
            List<CommonRoomRentalRequest> rentalRequests
        )
        {
            var reportsByCommonRoomId = rentalRequests
                .GroupBy(r => r.CommonRoomId)
                .Select(
                    group => new CommonRoomRentalRequestsReportDto(
                        _commonRoomMapper.MapToDto(commonRooms[group.Key]),
                        group.Select(_rentalRequestMapper.MapToDto).ToList()
                    )
                )
                .ToDictionary(report => report.CommonRoom.Id);

            return commonRooms
                .Values
                .Select(commonRoom => reportsByCommonRoomId.TryGetValue(commonRoom.Id, out var report)
                    ? report
                    : new CommonRoomRentalRequestsReportDto(
                        _commonRoomMapper.MapToDto(commonRoom),
                        new List<CommonRoomRentalRequestDto>()
                    ))
                .ToList();
        }
    }
}
