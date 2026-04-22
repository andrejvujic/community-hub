namespace CommunityHub.Application.Domain.NoticeBoard
{
    public class AdTimeSlot
    {
        public long Id { get; set; }
        public long AdId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public AdTimeSlot(
            long adId,
            DateTime startTime,
            DateTime endTime,
            long id = 0
        ) {
            Id = id;
            AdId = adId;
            StartTime = startTime;
            EndTime = endTime;
        }

        public AdTimeSlot CopyWith(
            long? id = null,
            long? adId = null,
            DateTime? startTime = null,
            DateTime? endTime = null
        ) => new AdTimeSlot(
                    adId ?? AdId,
                    startTime ?? StartTime,
                    endTime ?? EndTime,
                    id ?? Id
                 );
    }
}
