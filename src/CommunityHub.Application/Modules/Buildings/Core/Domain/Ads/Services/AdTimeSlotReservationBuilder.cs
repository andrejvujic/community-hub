using CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;
namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.Services
{
    public class AdTimeSlotReservationBuilder: IAdTimeSlotReservationBuilder
    {
        private readonly IAdTimeSlotService _timeSlotService;

        public AdTimeSlotReservationBuilder(IAdTimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
        }

        public List<AdTimeSlotReservation>? CreateReservations(
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
