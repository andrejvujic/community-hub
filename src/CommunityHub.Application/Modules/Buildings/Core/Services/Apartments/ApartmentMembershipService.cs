using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Apartments
{
    public class ApartmentMembershipService
    {
        private readonly IApartmentMembershipRepository _apartmentMembershipRepository = Injector.CreateInstance<IApartmentMembershipRepository>();

        public List<ApartmentMembership> GetMemberships(long residentId)
            => _apartmentMembershipRepository.GetByResident(residentId);

        public bool IsMember(long residentId, long apartmentId)
        {
            var memberships = _apartmentMembershipRepository.GetByApartment(apartmentId);
            return memberships.Any(membership => membership.Resident.Id == residentId);
        }

        public bool IsOccupied(long apartmentId)
        {
            var memberships = _apartmentMembershipRepository.GetByApartment(apartmentId);
            return memberships.Count > 0;
        }
    }
}