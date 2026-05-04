using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots
{
    public interface IAdTimeSlotService
    {
        public void Create(AdTimeSlot timeSlot);
    }
}
