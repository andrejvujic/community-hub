using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using CommunityHub.Application.MappersDto.Users;

namespace CommunityHub.Application.MappersDto.CommonRooms
{
    public class CommonRoomRentalRequestMapper : IMapper<CommonRoomRentalRequest, CommonRoomRentalRequestDto>
    {
        private readonly CommonRoomMapper _commonRoomMapper = new();
        private readonly UserMapper _userMapper = new();
        public CommonRoomRentalRequestDto MapToDto(CommonRoomRentalRequest rentalRequest)
        {
            if (rentalRequest == null) throw new ArgumentNullException("Couldn't convert null reference to CommonRoomRentalRequestDto");

            return new CommonRoomRentalRequestDto(
                id: rentalRequest.Id,
                commonRoomId: rentalRequest.CommonRoomId,
                commonRoom: rentalRequest.CommonRoom is not null ? _commonRoomMapper.MapToDto(rentalRequest.CommonRoom) : null,
                residentId: rentalRequest.ResidentId,
                resident: rentalRequest.Resident is not null ? _userMapper.MapToDto(rentalRequest.Resident) : null,
                requestedStartDate: rentalRequest.RequestedStartDate,
                requestedEndDate: rentalRequest.RequestedEndDate,
                proposedStartDate: rentalRequest.ProposedStartDate,
                proposedEndDate: rentalRequest.ProposedEndDate,
                assignedDate: rentalRequest.AssignedDate,
                status: rentalRequest.Status
            );
        }
        public CommonRoomRentalRequest MapToEntity(CommonRoomRentalRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException("Couldn't convert null reference to CommonRoomRentalRequest");

            return new CommonRoomRentalRequest(
                dto.ResidentId,
                dto.CommonRoomId,
                dto.RequestedStartDate,
                dto.RequestedEndDate,
                dto.Resident != null ? _userMapper.MapToEntity(dto.Resident) : null,
                dto.CommonRoom != null ? _commonRoomMapper.MapToEntity(dto.CommonRoom) : null,
                dto.ProposedStartDate,
                dto.ProposedEndDate,
                dto.AssignedDate,
                dto.Status,
                dto.Id
            );
        }
    }
}