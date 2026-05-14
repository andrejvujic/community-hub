using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.Domain.Apartments.RepositoryInterfaces;
using CommunityHub.Application.Helpers.Sorting;
using CommunityHub.Application.MappersDto.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;
using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Services.Apartments
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
        private readonly ApartmentAccessRequestMapper _apartmentAccessRequestMapper = new();

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

        public List<ApartmentAccessRequestDto> GetByUserAndBuilding(long userId, long buildingId, Status? status = null)
        {
            var apartmentAccessRequests = _apartmentAccessRequestRepository.GetByUserAndBuilding(userId, buildingId, status);
            List<ApartmentAccessRequestDto> apartmentAccessRequestsDto = [];

            foreach (var request in apartmentAccessRequests)
                apartmentAccessRequestsDto.Add(_apartmentAccessRequestMapper.MapToDto(request));

            return apartmentAccessRequestsDto;
        }

        public bool UpdateStatus(ApartmentAccessRequestDto requestDto, Status newStatus, string comment)
        {
            var domainRequest = _apartmentAccessRequestMapper.MapToDomain(requestDto);

            var updatedRequest = domainRequest.CopyWith(status: newStatus, comment: comment);

            int rowsAffected = _apartmentAccessRequestRepository.Update(updatedRequest);

            if (rowsAffected == 0) return false;

            if (newStatus == Status.Accepted)
            {
                ApartmentMembership membership = new(updatedRequest.Applicant!, updatedRequest.Apartment!, DateTime.Now);
                _apartmentMembershipService.Create(membership);
            }

            return true;
        }
    }
}
