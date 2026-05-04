using CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.RepositoryInterfaces;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Core.Services.Ads
{
    public class AdTimeSlotService : IAdTimeSlotService
    {
        private readonly IAdTimeSlotRepository _adTimeSlotRepository;

        public AdTimeSlotService(IAdTimeSlotRepository adTimeSlotRepository)
        {
            _adTimeSlotRepository = adTimeSlotRepository;
        }

        public void Create(AdTimeSlot timeSlot)
        {
            _adTimeSlotRepository.Create(timeSlot);
        }
    }
}
