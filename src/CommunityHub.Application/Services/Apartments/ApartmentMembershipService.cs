using CommunityHub.Application.Domain.Apartments;
using CommunityHub.Application.Domain.Apartments.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.Apartments;
using CommunityHub.Application.Services.Dtos.Apartments;
using System.Collections.ObjectModel;

namespace CommunityHub.Application.Services.Apartments
{
    public class ApartmentMembershipService
    {
        private readonly IApartmentMembershipRepository _apartmentMembershipRepository = Injector.CreateInstance<IApartmentMembershipRepository>();
        private readonly ApartmentMembershipMapper _apartmentMembershipMapper = new();
        public List<ApartmentMembership> GetMemberships(long residentId)
            => _apartmentMembershipRepository.GetByResident(residentId);

        public bool IsMember(long residentId, long apartmentId)
        {
            return _apartmentMembershipRepository.Exists(residentId, apartmentId);
            // var memberships = _apartmentMembershipRepository.GetByApartment(apartmentId);
            // return memberships.Any(membership => membership.Resident.Id == residentId);
        }

        public bool HasMembers(long apartmentId)
        {
            return _apartmentMembershipRepository.Exists(apartmentId);
            // var memberships = _apartmentMembershipRepository.GetByApartment(apartmentId);
            // return memberships.Count > 0;
        }

        public List<ApartmentMembershipDto> GetByUserAndBuilding(long userId, long buildingId)
        {
            var memberships = _apartmentMembershipRepository.GetByUserAndBuilding(userId, buildingId);
            var membershipDtos = memberships.Select(_apartmentMembershipMapper.MapToDto).ToList();
            return membershipDtos;
        }

        public ApartmentMembership Create(ApartmentMembership membership)
            => _apartmentMembershipRepository.Create(membership);

    }
}

