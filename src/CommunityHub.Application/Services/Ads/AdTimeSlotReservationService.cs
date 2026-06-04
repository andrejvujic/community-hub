using CommunityHub.Application.Domain.Ads.Services;
using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.MappersDto.Ads;
using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Application.Services.Ads
{
    public class AdTimeSlotReservationService: BaseService<AdTimeSlot, AdTimeSlotDto>
    {
        private readonly IAdTimeSlotReservationRepository _timeSlotReservationRepository;
        private readonly AdTimeSlotReservationGenerator _timeSlotReservationGenerator;
        private readonly AdTimeSlotMapper _adTimeSlotMapper = new();
        public AdTimeSlotReservationService(
            IAdTimeSlotRepository timeSlotRepository,
            IAdTimeSlotReservationRepository timeSlotReservationRepository
        ) : base(mapper: new AdTimeSlotMapper())
        {
            _timeSlotReservationRepository = timeSlotReservationRepository;
            _timeSlotReservationGenerator = new(timeSlotRepository);
        }

        public List<AdTimeSlotReservation>? Reserve(long userId, List<AdTimeSlotDto> timeSlots)
        {
            var reservations = _timeSlotReservationGenerator.Generate(
                userId,
                MapToEntity(timeSlots)
            );

            if (reservations is null) return null;

            _timeSlotReservationRepository.CreateMany(reservations);

            return reservations;
        }
    }
}
