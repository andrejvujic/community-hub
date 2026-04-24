namespace CommunityHub.Application.Domain.NoticeBoard
{
    public class AdReservation
    {
        public Ad Ad { get; }
        public List<AdTimeSlotGroup> TimeSlotGroups { get; }

        public AdReservation(Ad ad, List<AdTimeSlotGroup> timeSlotGroups) {
            Ad = ad;
            TimeSlotGroups = timeSlotGroups;
        }
    }
}
