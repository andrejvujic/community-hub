using CommunityHub.Application.Domain.Neighborhoods.CityFacilities;

namespace CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.CityFacilities
{
    public interface IFacilityReservationRepository
    {
        List<FacilityReservation> GetByNeighborhoodId(long neighborhoodId);
        List<FacilityReservation> GetByFacilityId(long facilityId);
        FacilityReservation Create(FacilityReservation reservation);
    }
}
