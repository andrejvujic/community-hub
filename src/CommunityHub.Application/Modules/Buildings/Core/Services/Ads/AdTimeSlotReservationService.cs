using CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.Services;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;
using CommunityHub.Application.Modules.Shared.Core.Domain.Users.RepositoryInterfaces;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Ads
{
    public class AdTimeSlotReservationService : IAdTimeSlotReservationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdTimeSlotReservationRepository _timeSlotReservationRepository;
        private readonly IAdTimeSlotReservationBuilder _timeSlotReservationBuilder;

        public AdTimeSlotReservationService(
            IUserRepository userRepository,
            IAdTimeSlotReservationRepository timeSlotReservationRepository,
            IAdTimeSlotReservationBuilder timeSlotReservationBuilder
        )
        {
            _userRepository = userRepository;
            _timeSlotReservationRepository = timeSlotReservationRepository;
            _timeSlotReservationBuilder = timeSlotReservationBuilder;
        }

        public List<AdTimeSlotReservation>? Reserve(long userId, List<AdTimeSlot> timeSlots)
        {
            var user = _userRepository.Get(userId);

            var reservations = _timeSlotReservationBuilder.CreateReservations(userId, timeSlots);
            if (reservations is null) return null;

            _timeSlotReservationRepository.CreateMany(reservations);

            return reservations;
        }
    }
}
