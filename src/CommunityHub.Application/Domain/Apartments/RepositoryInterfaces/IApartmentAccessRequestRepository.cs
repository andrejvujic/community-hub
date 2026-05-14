using CommunityHub.Application.Domain.Apartments.AccessRequests;
using CommunityHub.Application.Helpers.Sorting;

namespace CommunityHub.Application.Domain.Apartments.RepositoryInterfaces
{
    public interface IApartmentAccessRequestRepository
    {
        public ApartmentAccessRequest Create(ApartmentAccessRequest request);
        public void Delete(long requestId);
        public List<ApartmentAccessRequest> GetByApplicant(
            long id,
            Status? status = null,
            RequestOrder order = RequestOrder.Unspecified
        );
        public List<ApartmentAccessRequest> GetByApplicantAndApartment(
            long applicantId,
            long apartmentId,
            Status? status = null
        );

        public List<ApartmentAccessRequest> GetByUserAndBuilding(
            long userId,
            long buildingId,
            Status? status = null
        );
        public bool HasPendingRequest(long userId, long apartmentId);
        public int Update(ApartmentAccessRequest request);
        
    }
}