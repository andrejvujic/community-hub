using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.Services
{
    public class AdTimeSlotGenerator : IAdTimeSlotGenerator
    {
        public List<AdTimeSlot> GenerateTimeSlots(
            Ad ad,
            int firstTimeSlotStartHour = 16,
            int timeSlotCount = 4
        )
        {
            var timeSlots = new List<AdTimeSlot>();
            var currentDay = ad.StartDate.Date;

            while (currentDay <= ad.EndDate.Date)
            {
                for (int i = 0; i < timeSlotCount; ++i)
                {
                    var slotStartTime = currentDay.AddHours(firstTimeSlotStartHour + i);
                    var slotEndTime = slotStartTime.AddHours(1);

                    timeSlots.Add(
                        new AdTimeSlot(
                            ad.Id,
                            startTime: slotStartTime,
                            endTime: slotEndTime
                        )
                    );
                }

                currentDay = currentDay.AddDays(1);
            }

            return timeSlots;
        }
    }
}
