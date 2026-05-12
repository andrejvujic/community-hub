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
            if (rentalRequest == null) throw new ArgumentNullException(nameof(rentalRequest));

            return new CommonRoomRentalRequestDto(
                rentalRequest.Id,
                rentalRequest.CommonRoomId,
                rentalRequest.CommonRoom != null ? _commonRoomMapper.MapToDto(rentalRequest.CommonRoom) : null,
                rentalRequest.ResidentId,
                rentalRequest.Resident != null ? _userMapper.MapToDto(rentalRequest.Resident) : null,
                rentalRequest.RequestedStartDate,
                rentalRequest.RequestedEndDate,
                rentalRequest.ProposedStartDate,
                rentalRequest.ProposedEndDate,
                rentalRequest.AssignedDate,
                rentalRequest.Status
            );
        }
        public CommonRoomRentalRequest MapToDomain(CommonRoomRentalRequestDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new CommonRoomRentalRequest(
                dto.ResidentId,
                dto.CommonRoomId,
                dto.RequestedStartDate,
                dto.RequestedEndDate,
                dto.Resident != null ? _userMapper.MapToDomain(dto.Resident) : null,
                dto.CommonRoom != null ? _commonRoomMapper.MapToDomain(dto.CommonRoom) : null,
                dto.ProposedStartDate,
                dto.ProposedEndDate,
                dto.AssignedDate,
                dto.Status,
                dto.Id
            );
        }
    }
}