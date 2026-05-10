using CommunityHub.Application.Domain.Buildings.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.CommonRooms
{
    public enum RentalRequestValidationResult
    {
        Valid,
        PendingRequestExists
    }

    public class CommonRoomRentalRequestService
    {
        private readonly ICommonRoomRentalRequestRepository _rentalRequestRepository 
            = Injector.CreateInstance<ICommonRoomRentalRequestRepository>();

        public void Create(CommonRoomRentalRequest request)
            => _rentalRequestRepository.Create(request);

        public List<CommonRoomRentalRequest> GetByResidentAndCommonRoom(long residentId, long commonRoomId)
            => _rentalRequestRepository.GetByResidentAndCommonRoom(residentId, commonRoomId);

        public RentalRequestValidationResult ValidateRentalRequest(long residentId, long commonRoomId)
        {
            if (HasPendingRequest(residentId, commonRoomId))
                return RentalRequestValidationResult.PendingRequestExists;

            return RentalRequestValidationResult.Valid;
        }

        public bool HasPendingRequest(long residentId, long commonRoomId)
        {
            var requests = _rentalRequestRepository.GetByResidentAndCommonRoom(
                residentId,
                commonRoomId
            );
            return requests.Any(request => request.IsPending);
        }
    }
}
