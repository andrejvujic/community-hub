using CommunityHub.Application.Database.Repositories;

namespace CommunityHub.Application.Services
{
    public class FacilityBookingService
    {
        private readonly FacilityReservationRepository _reservationRepository;

        public FacilityBookingService(FacilityReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
    }
}
