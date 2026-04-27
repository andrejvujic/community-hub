using CommunityHub.Application.Database.Repositories;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.CityFacility;
using Xceed.Wpf.Toolkit.Core;

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

        public FacilityReservation BookSlot(CityFacility facility, Neighborhood neighborhood, AvailableFacilitySlot slot)
        {
            var reservation = new FacilityReservation(facility, neighborhood, slot.Start, slot.End);

            return _reservationRepository.Create(reservation);
        }
    }
}
