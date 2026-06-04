using CommunityHub.Application.Domain.Ads;
using CommunityHub.Application.Domain.Ads.TimeSlots;

namespace CommunityHub.Application.Domain.Ads.Services
{
    public class AdTimeSlotGenerator 
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

                timeSlots.AddRange(
                    GenerateTimeSlotsForDay(
                        day: currentDay,
                        ad: ad,
                        firstTimeSlotStartHour: firstTimeSlotStartHour,
                        timeSlotCount: timeSlotCount
                    )
                );
                currentDay = currentDay.AddDays(1);
            }

            return timeSlots;
        }

        private static List<AdTimeSlot> GenerateTimeSlotsForDay(
            DateTime day,
            Ad ad,
            int firstTimeSlotStartHour,
            int timeSlotCount
        )
        {
            var timeSlots = new List<AdTimeSlot>();

            for (int i = 0; i < timeSlotCount; ++i)
            {
                var slotStartTime = day.AddHours(firstTimeSlotStartHour + i);
                var slotEndTime = slotStartTime.AddHours(1);

                timeSlots.Add(
                    new AdTimeSlot(
                        ad.Id,
                        startTime: slotStartTime,
                        endTime: slotEndTime
                    )
                );
            }

            return timeSlots;
        }
    }
}
