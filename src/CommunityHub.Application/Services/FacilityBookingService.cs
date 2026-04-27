using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain.CityFacility;

namespace CommunityHub.Application.Services
{
    public class FacilityBookingService
    {
        private readonly FacilityReservationRepository _reservationRepository;

        public FacilityBookingService(FacilityReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public List<FacilityReservation> GetReservationsForNeighborhood(long neighborhoodId)
            => _reservationRepository.GetByNeighborhoodId(neighborhoodId);
    }
}
