using CommunityHub.Application.Domain.Ads.Services;
using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;

namespace CommunityHub.Application.Services.Ads
{
    public class AdTimeSlotReservationService
    {
        private readonly IAdTimeSlotReservationRepository _timeSlotReservationRepository;
        private readonly AdTimeSlotReservationGenerator _timeSlotReservationGenerator;

        public AdTimeSlotReservationService(
            IAdTimeSlotRepository timeSlotRepository,
            IAdTimeSlotReservationRepository timeSlotReservationRepository
        )
        {
            _timeSlotReservationRepository = timeSlotReservationRepository;
            _timeSlotReservationGenerator = new(timeSlotRepository);
        }

        public List<AdTimeSlotReservation>? Reserve(long userId, List<AdTimeSlot> timeSlots)
        {
            var reservations = _timeSlotReservationGenerator.Generate(userId, timeSlots);
            if (reservations is null) return null;

            _timeSlotReservationRepository.CreateMany(reservations);

            return reservations;
        }
    }
}
