using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.Domain.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Sorting;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Apartments
{
    public enum AccessRequestValidationResult
    {
        Valid,
        ApartmentNotFound,
        ApartmentOccupied,
        AlreadyMember,
        PendingRequestExists
    }

    public class ApartmentAccessRequestService
    {
        private readonly IApartmentRepository _apartmentRepository = Injector.CreateInstance<IApartmentRepository>();
        private readonly IApartmentAccessRequestRepository _apartmentAccessRequestRepository 
            = Injector.CreateInstance<IApartmentAccessRequestRepository>();

        private readonly ApartmentMembershipService _apartmentMembershipService = new();

        public void Create(ApartmentAccessRequest request)
            => _apartmentAccessRequestRepository.Create(request);

        public void Cancel(ApartmentAccessRequest request)
        {
            if (!request.IsPending)
                throw new InvalidOperationException("Only pendning requests can be cancelled.");

            _apartmentAccessRequestRepository.Delete(request.Id);
        }


        public List<ApartmentAccessRequest> GetByApplicant(
            long applicantId,
            Status? status = null,
            RequestOrder order = RequestOrder.Unspecified
        ) => _apartmentAccessRequestRepository.GetByApplicant(
                applicantId,
                status: status,
                order: order
              );

        public AccessRequestValidationResult ValidateAccessRequest(long residentId, long buildingId, int apartmentNumber)
        {
            var apartmentId = _apartmentRepository.FindIdByBuildingAndNumber(buildingId, apartmentNumber);
            if (apartmentId == null)
                return AccessRequestValidationResult.ApartmentNotFound;

            if (HasPendingRequest(residentId, apartmentId.Value))
                return AccessRequestValidationResult.PendingRequestExists;

            if (_apartmentMembershipService.IsMember(residentId, apartmentId.Value))
                return AccessRequestValidationResult.AlreadyMember;

            if (_apartmentMembershipService.HasMembers(apartmentId.Value))
                return AccessRequestValidationResult.ApartmentOccupied;

            return AccessRequestValidationResult.Valid;
        }

        public bool HasPendingRequest(long residentId, long apartmentId)
        {
            var pendningRequests = _apartmentAccessRequestRepository.GetByApplicantAndApartment(
                residentId,
                apartmentId,
                status: Status.Waiting
            );
            return pendningRequests.Count > 0;
        }
    }
}
