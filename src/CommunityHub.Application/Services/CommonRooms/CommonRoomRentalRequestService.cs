using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.CommonRooms.RepositoryInterfaces;

namespace CommunityHub.Application.Services.CommonRooms
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

        public void Cancel(CommonRoomRentalRequest request)
        {
            if (!request.IsPending)
                throw new InvalidOperationException("Only pending requests can be cancelled.");

            _rentalRequestRepository.Delete(request.Id);
        }

        public CommonRoomRentalRequest AcceptProposedDateRange(CommonRoomRentalRequest request)
        {
            if (!request.HasProposedDateRange)
                throw new InvalidOperationException("Request doesn't have a proposed date range.");

            request.AcceptProposedDateRange();
            _rentalRequestRepository.Update(request);

            return request;
        }

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
