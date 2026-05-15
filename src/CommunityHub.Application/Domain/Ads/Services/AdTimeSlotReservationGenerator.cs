using CommunityHub.Application.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Domain.Ads.TimeSlots;
using CommunityHub.Application.Services.Ads;
namespace CommunityHub.Application.Domain.Ads.Services
{
    public class AdTimeSlotReservationGenerator
    {
        private readonly AdTimeSlotService _timeSlotService;

        public AdTimeSlotReservationGenerator(IAdTimeSlotRepository timeSlotRepository)
        {
            _timeSlotService = new(timeSlotRepository);
        }

        public List<AdTimeSlotReservation>? Generate(
            long userId,
            List<AdTimeSlot> timeSlots
        )
        {
            var pairs = new List<AdTimeSlotReservation>();

            foreach (var timeSlot in timeSlots)
            {
                var matchingTimeSlot = _timeSlotService.FindMatchingTimeSlotForAuthor(
                    userId,
                    referenceTimeSlot: timeSlot
                );

                if (matchingTimeSlot is null) return null;

                pairs.Add(
                    new AdTimeSlotReservation(
                        offerTimeSlotId: timeSlot.Ad!.IsOffer ? timeSlot.Id : matchingTimeSlot.Id,
                        requestTimeSlotId: timeSlot.Ad!.IsRequest ? timeSlot.Id : matchingTimeSlot.Id
                    )
                );
            }

            return pairs;
        }
    }
}
