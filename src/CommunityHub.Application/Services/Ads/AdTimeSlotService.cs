using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Domain.RepositoryInterfaces.Ads;
using CommunityHub.Application.MappersDto.Ads;
using CommunityHub.Application.Services.Dtos.Ads;

namespace CommunityHub.Application.Services.Ads
{
    public class AdTimeSlotService: BaseService<AdTimeSlot, AdTimeSlotDto>
    {
        private readonly IAdTimeSlotRepository _timeSlotRepository;

        public AdTimeSlotService(IAdTimeSlotRepository timeSlotRepository) : base(mapper: new AdTimeSlotMapper())
        {
            _timeSlotRepository = timeSlotRepository;
        }

        public void Create(AdTimeSlot timeSlot) =>
            _timeSlotRepository.Create(timeSlot);

        public void CreateMany(List<AdTimeSlot> timeSlots) =>
            _timeSlotRepository.CreateMany(timeSlots);

        public AdTimeSlot? FindMatchingTimeSlotForAuthor(long authorId, AdTimeSlot referenceTimeSlot)
        {
            var timeSlots = _timeSlotRepository.GetTimeSlotsByAuthor(authorId);
            return timeSlots.FirstOrDefault(
                timeSlot => timeSlot.Matches(referenceTimeSlot)
            );
        }

        public List<AdTimeSlotDto> GetReservableTimeSlotsForUser(long adId, long userId)
        {
            var adTimeSlots = _timeSlotRepository.GetTimeSlotsByAd(adId);
            var userTimeSlots = _timeSlotRepository.GetTimeSlotsByAuthor(userId);

            var reservableTimeSlots = adTimeSlots.Where(
                adTimeSlot => userTimeSlots.Any(
                    userTimeSlot => userTimeSlot.Matches(adTimeSlot)
                )
            ).ToList();

            return MapToDto(reservableTimeSlots);
        }

        public List<AdTimeSlotDto> GetReservedTimeSlotsForAd(long adId)
        {
            var timeSlots = _timeSlotRepository.GetTimeSlotsByAd(adId);
            return MapToDto(
                timeSlots.Where(timeSlot => timeSlot.IsReserved).ToList()
            );
        }
    }
}
